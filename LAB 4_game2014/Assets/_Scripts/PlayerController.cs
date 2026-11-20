using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveInput;
    private float joystickInput;
    private bool isGrounded;
    private bool jumpPressed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Read keyboard input or mobile joystick input
        float keyboardInput = Input.GetAxisRaw("Horizontal");
        moveInput = Mathf.Abs(joystickInput) > 0.05f ? joystickInput : keyboardInput;

        // Flip character sprite left or right
        if (moveInput > 0.05f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < -0.05f)
        {
            spriteRenderer.flipX = true;
        }

        // Jump with Spacebar / W / Up Arrow
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpPressed = true;
        }

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Check if player feet are touching the ground layer
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // Apply horizontal movement
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Apply jump force when grounded
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        jumpPressed = false;
    }

    // Called by the mobile joystick to move left and right
    public void SetJoystickInput(float horizontal)
    {
        joystickInput = horizontal;
    }

    // Called by the mobile jump button or joystick up
    public void OnJumpButton()
    {
        jumpPressed = true;
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
