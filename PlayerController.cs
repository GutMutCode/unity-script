using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 32f;
    public float acceleration = 50f;
    public float deceleration = 30f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private Vector2 targetVelocity;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Create ground check object if it doesn't exist
        if (groundCheck == null)
        {
            GameObject check = new GameObject("GroundCheck");
            check.transform.parent = transform;
            check.transform.localPosition = new Vector2(0, -0.5f); // Adjust based on your character's size
            groundCheck = check.transform;
        }
    }

    void Update()
    {
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Get input axes
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Create movement vector and normalize it
        Vector2 inputDirection = new Vector2(moveHorizontal, 0).normalized;

        // Calculate target velocity
        targetVelocity = inputDirection * maxSpeed;

        // Calculate acceleration or deceleration based on input
        float accelerationRate = inputDirection.magnitude > 0 ? acceleration : deceleration;

        // Smoothly interpolate current velocity towards target velocity
        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            targetVelocity,
            accelerationRate * Time.deltaTime
        );

        // Apply horizontal movement only (vertical movement is handled by physics/jumping)
        Vector2 newPosition = rb.position + new Vector2(currentVelocity.x * Time.deltaTime, 0);
        rb.MovePosition(newPosition);
    }

    // Optional: Add method to visualize movement values in the Unity Editor
    void OnGUI()
    {
        GUILayout.Label($"Current Speed: {currentVelocity.magnitude:F2}");
        GUILayout.Label($"Target Speed: {targetVelocity.magnitude:F2}");
        GUILayout.Label($"Grounded: {isGrounded}");
    }
}
