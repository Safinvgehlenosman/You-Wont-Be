using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;

    private PlayerInteract interact;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        interact = GetComponentInChildren<PlayerInteract>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.ToggleFly.performed += ctx => motor.ToggleFly();
        onFoot.Interact.performed += ctx => interact.DoInteract();
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
        motor.ProcessMove(moveInput);
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
}