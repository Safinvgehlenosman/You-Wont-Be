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

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.ToggleFly.performed += ctx => motor.ToggleFly();
    }

    void OnEnable()
    {
        onFoot.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        onFoot.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void FixedUpdate()
    {
        Vector2 moveInput = onFoot.Movement.ReadValue<Vector2>();
        
        // DEBUG: See if the keys are actually being registered
        if (moveInput.magnitude > 0.01f)
        {
            Debug.Log($"<color=white>InputManager:</color> Read WASD: {moveInput}");
        }

        motor.ProcessMove(moveInput);
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
}