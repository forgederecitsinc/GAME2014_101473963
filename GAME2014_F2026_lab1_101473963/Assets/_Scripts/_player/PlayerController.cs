using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Camera mainCamera;
    
    // Drag your GameOverText object into this slot in the Inspector
    public GameObject gameOverUI;

    void Start()
    {
        mainCamera = Camera.main;
        // Ensure the game runs at normal speed when the scene loads
        Time.timeScale = 1f; 
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(touch.position);
            targetPosition.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // This method runs automatically when a Rigidbody2D enters a Trigger Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we hit has the "Enemy" tag
        if (collision.CompareTag("Enemy"))
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
    }
}
