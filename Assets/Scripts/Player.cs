using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckDistance = 0; 

    private Rigidbody rb;
    private bool isGrounded;
    public int maxJumps = 2;
    private int currentJumps = 0;
    private bool hasJumped; // Flag to prevent multiple jumps within one key press

    void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJump.AddListener(OnJumpAction);
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
    }

    private void MovePlayer(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
        // Apply force relative to the player's current rotation
        rb.AddForce(transform.TransformDirection(moveDirection) * speed, ForceMode.Force); 
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
    }
}