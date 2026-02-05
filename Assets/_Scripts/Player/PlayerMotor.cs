using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    public float gravity = -9.8f;
    public bool isGrounded;
    public bool isFlying = false;

    public Animator anim;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [Range(0.01f, 0.5f)] public float smoothTime = 0.15f; // Controls how fast the character "reaches" top speed

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (!isFlying) 
            isGrounded = controller.isGrounded;
    }

    public void ToggleFly()
    {
        isFlying = !isFlying;
        playerVelocity = Vector3.zero;
    }

    public void ProcessMove(Vector2 input)
    {
        // THE SMOOTHING FIX: Calculate the smoothed vector first
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothTime);

        // 1. UPDATE ANIMATOR WITH DAMPING
        if (anim != null)
        {
            // We now pass currentInputVector instead of raw input
            // 0.1f is the internal damping time; Time.deltaTime keeps it frame-rate independent
            anim.SetFloat("MoveX", currentInputVector.x, 0.1f, Time.deltaTime);
            anim.SetFloat("MoveY", currentInputVector.y, 0.1f, Time.deltaTime);
        }

        // 2. MOVEMENT LOGIC (Uses smoothed values for character weight)
        Vector3 moveDirection = new Vector3(currentInputVector.x, 0, currentInputVector.y);

        if (isFlying)
        {
            Transform camTransform = GetComponentInChildren<Camera>().transform;
            Vector3 flyDir = (camTransform.forward * moveDirection.z) + (camTransform.right * moveDirection.x);
            transform.position += flyDir * (speed * 2f) * Time.deltaTime;
            return; 
        }

        // Move the controller based on smoothed direction
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        
        // Gravity logic
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