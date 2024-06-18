using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
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
    private Vector3 initialCameraPosition;

    private void Start()
    {
        initialCameraPosition = cameraOffset;
        cam.transform.localPosition = initialCameraPosition;
    }

    private void Update()
    {
        HandleCrouchTransition();
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        //apply to camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
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
            Debug.Log("Crouching");
            crouchTimer += Time.deltaTime * crouchTransitionSpeed;
            float targetHeight = isCrouching ? cameraOffset.y - crouchHeightAdjustment : cameraOffset.y;
            cam.transform.localPosition = new Vector3(cameraOffset.x, Mathf.Lerp(cam.transform.localPosition.y, targetHeight, crouchTimer), cameraOffset.z);

            if (crouchTimer >= 1f)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
                cam.transform.localPosition = new Vector3(cameraOffset.x, targetHeight, cameraOffset.z);
            }
        }
    }
}
