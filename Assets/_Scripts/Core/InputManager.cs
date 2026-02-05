using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        // Jump callback
        onFoot.Jump.performed += ctx => motor.Jump();
        
        // Crouch callback (optional, we will add logic later)
        // onFoot.Crouch.performed += ctx => motor.Crouch();
    }

    // 1. LOCK THE CURSOR (The Fix for your mouse issue)
    void OnEnable()
    {
        onFoot.Enable();
        // Hide the mouse and lock it to the center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        onFoot.Disable();
        // Release the mouse when disabled
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 2. CONNECT THE WIRES (Pass the data every frame)
    void FixedUpdate()
    {
        // Tell the PlayerMotor to move using the value from our Movement Action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        // Tell the PlayerLook to rotate using the value from our Look Action
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
}