using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.HudActions hud;

    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerShoot shoot;
    private PlayerUI playerUI;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        hud = playerInput.Hud;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        shoot = GetComponent<PlayerShoot>();
        playerUI = GetComponent<PlayerUI>();

        //Jumping
        onFoot.Jump.performed += ctx => motor.Jump();

        //Crouching
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Crouch.canceled += ctx => motor.Crouch();

        //Sprinting
        onFoot.Sprint.started += ctx => motor.SetSprinting(true);
        onFoot.Sprint.canceled += ctx => motor.SetSprinting(false);

        // Shooting
        onFoot.Shoot.started += ctx => shoot.SetShooting(true);
        onFoot.Shoot.canceled += ctx => shoot.SetShooting(false);

        // Score
        hud.Players.started += ctx => playerUI.ShowPlayersScore(true);
        hud.Players.canceled += ctx => playerUI.ShowPlayersScore(false);
    }

    private void Update()
    {
        if (!motor.IsLocalPlayer) return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!motor.IsLocalPlayer) return;
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        if (!motor.IsLocalPlayer) return;
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
        hud.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
        hud.Disable();
    }
}
