using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 32f;
    public float acceleration = 50f;
    public float deceleration = 30f;

    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private Vector2 targetVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input axes
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Create movement vector and normalize it
        Vector2 inputDirection = new Vector2(moveHorizontal, moveVertical).normalized;

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

        // Apply movement
        rb.MovePosition(rb.position + currentVelocity * Time.deltaTime);
    }

    // Optional: Add method to visualize movement values in the Unity Editor
    void OnGUI()
    {
        GUILayout.Label($"Current Speed: {currentVelocity.magnitude:F2}");
        GUILayout.Label($"Target Speed: {targetVelocity.magnitude:F2}");
    }
}
