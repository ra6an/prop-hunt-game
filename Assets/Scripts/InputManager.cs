using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerShoot shoot;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        shoot = GetComponent<PlayerShoot>();

        //Jumping
        onFoot.Jump.performed += ctx => motor.Jump();

        //Crouching
        //onFoot.Crouch.started += ctx => motor.SetCrouching(true);
        //onFoot.Crouch.canceled += ctx => motor.SetCrouching(false);
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Crouch.canceled += ctx => motor.Crouch();

        //Sprinting
        onFoot.Sprint.started += ctx => motor.SetSprinting(true);
        onFoot.Sprint.canceled += ctx => motor.SetSprinting(false);

        // Shooting
        onFoot.Shoot.started += ctx => shoot.SetShooting(true);
        onFoot.Shoot.canceled += ctx => shoot.SetShooting(false);
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
