using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 12f;
    public float topEdge = 10f;
    public int points = 100;

    void Update()
    {
        // Move upward every frame
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // When bullet moves beyond the top of the screen, return it to the pool
        if (transform.position.y >= topEdge)
        {
            ReturnToPool();
        }
    }

    // Handles collision with enemies/obstacles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Award points for hitting an enemy
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(points);
            }

            // Return the enemy back to the enemy object pool
            if (ObjectPool.Instance != null)
            {
                ObjectPool.Instance.ReturnEnemy(collision.gameObject);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }

            // Return this bullet back to the bullet object pool
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        if (BulletPool.Instance != null)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
