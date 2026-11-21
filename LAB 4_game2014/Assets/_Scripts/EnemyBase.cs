using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float moveSpeed = 2.5f;
    public int damage = 1;

    [Header("Patrol Detection")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.6f;
    public Transform wallCheck;
    public float wallCheckDistance = 0.25f;
    public LayerMask groundLayer;

    [SerializeField] protected bool movingRight = false;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate()
    {
        Patrol();
    }

    protected virtual void Patrol()
    {
        if (rb == null) return;

        float direction = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        bool hasGroundAhead = true;
        if (groundCheck != null)
        {
            RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
            hasGroundAhead = groundHit.collider != null;
        }

        bool hitWall = false;
        if (wallCheck != null)
        {
            Vector2 wallDir = movingRight ? Vector2.right : Vector2.left;
            RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, wallDir, wallCheckDistance, groundLayer);
            hitWall = wallHit.collider != null;
        }

        if (!hasGroundAhead || hitWall)
        {
            Flip();
        }
    }

    protected virtual void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnHitPlayer(collision.gameObject);
        }
    }

    protected virtual void OnHitPlayer(GameObject playerObj)
    {
        // Base hook for damaging the player (extended in Lab 5 / Lab 6)
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Vector3 dir = movingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + dir * wallCheckDistance);
        }
    }
}
