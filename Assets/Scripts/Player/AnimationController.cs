using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : NetworkBehaviour
{
    [SerializeField]
    private GameObject player;
    private Animator animator;
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;


    private void Awake()
    {
        //Debug.Log("Awake");
        //animator = GetComponent<Animator>();
        animator = player.GetComponent<Animator>();
        motor = GetComponent<PlayerMotor>();
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
    }
    private void OnEnable()
    {
        //Debug.Log("On enable called");
        playerInput.Enable();
        SubscribeActions();
    }

    private void OnDisable()
    {
        //Debug.Log("On disable called");
        playerInput.Disable();
    }

    private void SubscribeActions()
    {
        onFoot.Movement.performed += OnMove;
        onFoot.Movement.canceled += OnMoveCancel;
        onFoot.Jump.performed += ctx => OnJump();
        onFoot.Sprint.performed += ctx => OnRun();
        onFoot.Sprint.canceled += ctx => OnRunCancel();
        onFoot.Crouch.performed += ctx => OnCrouch();
        onFoot.Crouch.canceled += ctx => OnCrouchingCancel();
    }

    private void Start()
    {
    }

    private float ReturnValue(float value) 
    {
        float returnValue;
        if(value == 0)
        {
            returnValue = 0;
        } else
        {
            returnValue = value > 0 ? 1 : -1;
        }
        return returnValue;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        float xAxis = ReturnValue(input.x);
        float yAxis = ReturnValue(input.y);
        animator.SetFloat("MoveX", xAxis);
        animator.SetFloat("MoveY", yAxis);
        animator.SetBool("IsWalking", true);
    }

    public void OnMoveCancel(InputAction.CallbackContext context)
    {
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", 0);
        animator.SetBool("IsWalking", false);
    }
    
    private void OnRun()
    {
        animator.SetBool("IsRunning", true);
    }

    private void OnRunCancel()
    {
        animator.SetBool("IsRunning", false);
    }

    public void OnJump()
    {
        if(motor.IsPlayerGrounded())
        {
            animator.SetTrigger("Jump");
        }
    }

    public void OnCrouch()
    {
        animator.SetBool("Crouching", true);
    }

    public void OnCrouchingCancel()
    {
        animator.SetBool("Crouching", false);
    }
}
