using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    public float gravity = -9.8f;
    public bool isGrounded;

    public bool isFlying = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Only check grounded if we are NOT flying to avoid weird behavior
        if (!isFlying) 
            isGrounded = controller.isGrounded;
    }

    public void ToggleFly()
    {
        isFlying = !isFlying;
        
        // Reset speed so you don't keep momentum
        playerVelocity = Vector3.zero;
        
        // Note: We don't need to change detectCollisions anymore
        // because we will bypass the controller entirely when moving.
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        // --- GHOST MODE LOGIC ---
        if (isFlying)
        {
            Transform camTransform = GetComponentInChildren<Camera>().transform;
            
            // Calculate direction based on camera look direction
            Vector3 flyDir = (camTransform.forward * moveDirection.z) + (camTransform.right * moveDirection.x);
            
            // THE FIX: Use transform.position instead of controller.Move
            // This teleports the player, ignoring ALL walls and floors.
            transform.position += flyDir * (speed * 2f) * Time.deltaTime;
            
            return; // Stop here
        }
        // ------------------------

        // Standard Walking Logic
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(1.5f * -2f * gravity);
        }
    }
}