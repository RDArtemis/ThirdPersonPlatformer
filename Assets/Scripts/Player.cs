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
    [SerializeField] private float normalGravity = -9.81f;
    [SerializeField] private float risingGravity = -12f;
    [SerializeField] private float fallingGravity = -18f;
    [SerializeField] private LayerMask groundLayer;
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
        rb.useGravity = false; // Disable default gravity

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

        // 3. Apply the movement
        rb.AddForce(moveDirection * speed, ForceMode.Force);
    }
    void LateUpdate() // Use LateUpdate to ensure camera rotation happens after movement
    {
        if (cameraTransform == null) return; // Error checking

        // 1. Get the camera's forward vector (horizontal)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // 2. Calculate the rotation
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        // 3. Apply the rotation
        transform.rotation = targetRotation;
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

    void FixedUpdate()
    {
        // Ground check using a raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        //different gravity values to make jumps feel more real
        float currentGravity = normalGravity; // Default gravity

        if (rb.linearVelocity.y > 0) // Rising
        {
            currentGravity = risingGravity;
        }
        else if (rb.linearVelocity.y < 0) // Falling
        {
            currentGravity = fallingGravity;
        }

        rb.AddForce(Vector3.up * currentGravity, ForceMode.Acceleration); // Apply custom gravity
        
        if (!isGrounded) //add damping when jumping to make smoother forward jumps 
        {
            rb.linearDamping = 1f;
        }
        else
        {
            rb.linearDamping = 0f;
        }

    }

    void Update()
    {
        
        if (isGrounded)
        {
            currentJumps = 0; // Reset jumps when grounded
        }
        if (Input.GetKeyUp(KeyCode.Space)) // Check for KeyUp
        {
            hasJumped = false; // Reset the flag when the key is released
        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0) //variable jump height
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f, rb.linearVelocity.z);
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