using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerLook : NetworkBehaviour
{

    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public Vector3 cameraOffset = new Vector3 (0f, 1.7f, 0f);
    public float crouchHeightAdjustment = 0.5f;
    public float crouchTransitionSpeed = 5f;

    private bool isCrouching;
    private float crouchTimer = 0f;
    private bool lerpCrouch = false;
    public Vector3 initialCameraPosition;

    public NetworkVariable<float> cameraPosition = new NetworkVariable<float> ();

    [Header("Aim Sphere")]
    [SerializeField]
    private GameObject aimSphere;
    [SerializeField]
    private LayerMask interactableLayers;
    [SerializeField]
    private float maxDistance = 100f;

    private void Start()
    {
        initialCameraPosition = cameraOffset;
        cam.transform.localPosition = initialCameraPosition;
    }

    private void Update()
    {
        HandleCrouchTransition();
        PositionHelperSphere();

        if(!IsLocalPlayer)
        {
            Quaternion rot = Quaternion.Euler(cameraPosition.Value, 0f, 0f);
            cam.transform.localRotation = rot;

        }
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

            //calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        Quaternion rot = Quaternion.Euler(xRotation, 0f, 0f);
        cam.transform.localRotation = rot;

        //apply to camera
        if(IsLocalPlayer)
        {
            CameraRotateServerRpc(xRotation);
        }

        //rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    private void PositionHelperSphere()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, maxDistance, interactableLayers))
        {
            aimSphere.transform.position = hit.point;
        } else
        {
            aimSphere.transform.position = ray.GetPoint(maxDistance);
        }
    }

    public void SetCrouching(bool crouch)
    {
        isCrouching = crouch;
        crouchTimer = 0f;
        lerpCrouch = true;
    }

    private void HandleCrouchTransition()
    {
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime * crouchTransitionSpeed;
            float targetHeight = isCrouching ? initialCameraPosition.y - crouchHeightAdjustment : initialCameraPosition.y;
            cam.transform.localPosition = new Vector3(cameraOffset.x, Mathf.Lerp(cam.transform.localPosition.y, targetHeight, crouchTimer), cameraOffset.z);

            if (crouchTimer >= 1f)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
                cam.transform.localPosition = new Vector3(cameraOffset.x, targetHeight, cameraOffset.z);
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(!IsLocalPlayer)
        {
            cameraPosition.OnValueChanged += OnCameraRotate;
        }
    }

    public override void OnNetworkDespawn()
    {
        if(!IsLocalPlayer)
        {
            cameraPosition.OnValueChanged -= OnCameraRotate;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CameraRotateServerRpc(float rotation)
    {
        cameraPosition.Value = rotation;
    }

    private void OnCameraRotate(float oldValue, float newValue)
    {
        cam.transform.localRotation = Quaternion.Euler(newValue, 0f, 0f);
    }
}
