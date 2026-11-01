using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    private Camera mainCamera;

    [Header("UI Reference")]
    // Drag your GameOverText object into this slot in the Inspector
    public GameObject gameOverUI;
    public TextMeshProUGUI livesText;

    [Header("Life Management")]
    public int maxLives = 3;
    private int currentLives;
    public float invulnerabilityDuration = 1.0f;
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;

    [Header("Bullet Generation")]
    public float fireRate = 0.25f;
    public Transform bulletSpawnPoint;
    private float fireTimer = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the game runs at normal speed when the scene loads
        Time.timeScale = 1f;

        if (livesText == null)
        {
            GameObject obj = GameObject.Find("LivesText");
            if (obj != null)
            {
                livesText = obj.GetComponent<TextMeshProUGUI>();
            }
        }

        // Initialize player lives
        currentLives = maxLives;
        UpdateLivesUI();
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    // Handles touch controls (and mouse fallback for Editor testing)
    private void HandleMovement()
    {
        Vector3 inputPos = Vector3.zero;
        bool hasInput = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPos = touch.position;
            hasInput = true;
        }
        else if (Input.GetMouseButton(0))
        {
            inputPos = Input.mousePosition;
            hasInput = true;
        }

        if (hasInput && mainCamera != null)
        {
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(inputPos);
            targetPosition.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // Generates pooled bullets at regular intervals
    private void HandleShooting()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    private void Shoot()
    {
        Vector3 spawnPos = bulletSpawnPoint != null ? bulletSpawnPoint.position : transform.position + Vector3.up * 0.8f;

        BulletPool pool = BulletPool.Instance ?? FindObjectOfType<BulletPool>();
        if (pool != null)
        {
            pool.GetBullet(spawnPos);
        }
    }

    // This method runs automatically when an obstacle/enemy enters the player's trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we hit has the "Enemy" tag
        if (collision.CompareTag("Enemy"))
        {
            // Return the enemy back to the pool so it doesn't stay overlapping the player
            if (ObjectPool.Instance != null)
            {
                ObjectPool.Instance.ReturnEnemy(collision.gameObject);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }

            // Deduct life if not currently invulnerable
            if (!isInvulnerable)
            {
                TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    // Brief invulnerability with flashing effect after taking a hit
    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        if (spriteRenderer != null)
        {
            for (int i = 0; i < 5; i++)
            {
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(invulnerabilityDuration / 10f);
                spriteRenderer.enabled = true;
                yield return new WaitForSeconds(invulnerabilityDuration / 10f);
            }
        }
        else
        {
            yield return new WaitForSeconds(invulnerabilityDuration);
        }

        isInvulnerable = false;
    }

    private void GameOver()
    {
        // Turn on the Game Over text
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Freeze the game
        Time.timeScale = 0f;

        // Hide the player
        gameObject.SetActive(false);
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }
}
