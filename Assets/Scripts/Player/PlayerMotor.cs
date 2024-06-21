using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMotor : NetworkBehaviour
{
    private CharacterController controller;
    private PlayerLook playerLook;

    private Animator animator;
    private ClientNetworkAnimator clientNetworkAnimator;

    private Vector3 playerVelocity;
    private Vector3 currentVelocity;
    private bool isGrounded;
    public float gravity = -9.8f;

    [Header("Movement")]
    public float speed = 5f;
    public float sprintSpeed = 5.5f;
    public float airControlFactor = 0.3f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float airAcceleration = 5f;
    public float airDeceleration = 5f;
    public Vector3 moveDirection;

    [Header("Jumping")]
    public float jumpHeight = 3f;

    [Header("Crouching")]
    [SerializeField]
    public float crouchSpeed = 2f;
    [SerializeField]
    public float crouchHeight = 1f;
    [SerializeField]
    public float originalHeight = 1.8f;
    private float crouchCenterY;
    private float originalCenterY;
    [SerializeField]
    public float crouchTransitionSpeed = 5f;
    public float crouchTimer = 0f;
    public bool lerpCrouch = false;
    public bool sprinting = false;
    public NetworkVariable<bool> crouching = new NetworkVariable<bool>(false);

    private float currentSpeed = 0f;

    //TESTING PURPOSE
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(new Vector3(0f,0f,0f), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>(new Quaternion(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerLook = GetComponent<PlayerLook>();

        animator = GetComponentInChildren<Animator>();
        clientNetworkAnimator = GetComponentInChildren<ClientNetworkAnimator>();

        currentSpeed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {

        originalHeight = controller.height;
        originalCenterY = controller.center.y;
        crouchCenterY = crouchHeight / 2f;

        playerVelocity = Vector3.zero;

        crouching.OnValueChanged += OnCrouchChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            // Update position and rotation based on player input
            Vector3 newPosition = transform.position; // Your position calculation
            Quaternion newRotation = transform.rotation; // Your rotation calculation

            Position.Value = newPosition;
            Rotation.Value = newRotation;
            Debug.Log($"Owner - Position: {newPosition}, Rotation: {newRotation}");
        }
        else
        {
            // Apply synchronized position and rotation for clients
            Debug.Log($"Client - Position: {Position.Value}, Rotation: {Rotation.Value}");
            transform.position = Position.Value;
            transform.rotation = Rotation.Value;
        }

        HandleMovement();
        HandleCrouch();
    }

    public bool IsPlayerGrounded()
    {
        return isGrounded;
    }

    private void HandleMovement()
    {
        GroundCheck();

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (moveDirection == Vector3.zero && currentVelocity != Vector3.zero)
        {
            float decel = isGrounded ? deceleration : airDeceleration;
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, decel * Time.deltaTime);
        }
    }

    private void GroundCheck()
    {
        Vector3 start = transform.position + Vector3.up * 0.1f;
        float rayLength = 0.2f;

        isGrounded = Physics.Raycast(start, Vector3.down, rayLength);
    }

    private void HandleCrouch()
    {
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime * crouchTransitionSpeed;
            float targetHeight = crouching.Value ? crouchHeight : originalHeight;
            float targetCenterY = crouching.Value ? crouchCenterY : originalCenterY;
            float newHeight = Mathf.Lerp(controller.height, targetHeight, crouchTimer);
            float newCenterY = Mathf.Lerp(controller.center.y, targetCenterY, crouchTimer);

            controller.height = newHeight;
            controller.center = new Vector3(controller.center.x, newCenterY, controller.center.z);

            if (crouchTimer >= 1f)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 desiredMoveDirection = Vector3.zero;
        desiredMoveDirection.x = input.x;
        desiredMoveDirection.z = input.y;
        desiredMoveDirection = transform.TransformDirection(desiredMoveDirection).normalized;

        float accel = isGrounded ? acceleration : airAcceleration;
        float _fixedSpeed = crouching.Value ? crouchSpeed : currentSpeed;
        moveDirection = desiredMoveDirection * _fixedSpeed;

        currentVelocity = Vector3.MoveTowards(currentVelocity, moveDirection, accel * Time.deltaTime);
        controller.Move(currentVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);

            JumpServerRpc();
        }
    }

    public void Crouch()
    {
        if(IsLocalPlayer)
        {
            crouchTimer = 0f;
            lerpCrouch = true;
            CrouchServerRpc(!crouching.Value);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        originalHeight = controller.height;
        originalCenterY = controller.center.y;
        crouchCenterY = crouchHeight / 2f;

        playerVelocity = Vector3.zero;

        if (IsServer)
        {
            crouching.OnValueChanged += OnCrouchChanged;
        }
    }

    private void OnEnable()
    {
        crouching.OnValueChanged += OnCrouchChanged;
    }

    private void OnDisable()
    {
        crouching.OnValueChanged -= OnCrouchChanged;
    }

    [ServerRpc]
    private void CrouchServerRpc(bool isCrouching)
    {
        crouching.Value = isCrouching;
    }

    private void OnCrouchChanged(bool oldValue, bool newValue)
    {
        crouchTimer = 0f;
        lerpCrouch = true;

        if (playerLook != null)
        {
            playerLook.SetCrouching(newValue);
        }
    }

    [ServerRpc]
    private void JumpServerRpc()
    {
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
    }

    public void SetSprinting(bool isSprinting)
    {
        sprinting = isSprinting;
        currentSpeed = isSprinting ? sprintSpeed : speed;
    }
}
