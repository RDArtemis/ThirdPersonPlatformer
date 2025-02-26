using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckDistance = 1;
    [SerializeField] private float dashSpeed = 10f; // Speed of the dash
    [SerializeField] private float dashDuration = 0.2f; // Duration of the dash
    [SerializeField] private float dashCooldown = 1f; // Cooldown between dashes
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private bool isGrounded;
    public int maxJumps = 2;
    private int currentJumps = 0;
    private bool hasJumped; // Flag to prevent multiple jumps within one key press
    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;

    void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJump.AddListener(OnJumpAction);
        inputManager.OnDash.AddListener(OnDashAction);
        rb = GetComponent<Rigidbody>();

        //debugging/ error checking
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing!");
        }
        if (inputManager == null)
        {
            Debug.LogError("Input Manager is missing!");
        }
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned!");
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        // 1. Get the camera's forward and right vectors (ignoring vertical component)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f; // Zero out the vertical component
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f; // Zero out the vertical component
        cameraRight.Normalize();

        // 2. Calculate the movement direction relative to the camera
        Vector3 moveDirection = cameraForward * direction.y + cameraRight * direction.x;

        // 3. Apply the movement (using Rigidbody)
        rb.AddForce(moveDirection * speed, ForceMode.Force);
    }

    private void OnJumpAction()
    {
        if (currentJumps < maxJumps && !hasJumped) // Check flag
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            currentJumps++;
            hasJumped = true; // Set the flag
        }
    
    }
    private void OnDashAction()
    {
        if (!isDashing && dashCooldownTimer <= 0) // Check if not already dashing and cooldown is over
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown; // Start cooldown
        }
    }

    void Update()
    {
        // Ground check using a raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (isGrounded)
        {
            currentJumps = 0; // Reset jumps when grounded
        }
        if (Input.GetKeyUp(KeyCode.Space)) // Check for KeyUp
        {
            hasJumped = false; // Reset the flag when the key is released
        }
        if (isDashing)
        {
            // Calculate dash direction (forward relative to player)
            Vector3 dashDirection = transform.forward;

            // Apply the dash
            rb.linearVelocity = dashDirection * dashSpeed; // Directly set the velocity for a snappy dash

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
                // rb.velocity = Vector3.zero; // Stop the dash abruptly (optional)  
            }
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }
}