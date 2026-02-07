using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    [SerializeField] private bool isLocalPlayer = true;

    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput;
    public event Action JumpPressed;
    public event Action ToggleFlyPressed;
    public event Action InteractPressed;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        onFoot.Jump.performed += ctx => JumpPressed?.Invoke();
        onFoot.ToggleFly.performed += ctx => ToggleFlyPressed?.Invoke();
        onFoot.Interact.performed += ctx => InteractPressed?.Invoke();
    }

    void OnEnable()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        onFoot.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        onFoot.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Vector2 moveInput = onFoot.Movement.ReadValue<Vector2>();
        MoveInput?.Invoke(moveInput);
    }

    void LateUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        LookInput?.Invoke(onFoot.Look.ReadValue<Vector2>());
    }
}