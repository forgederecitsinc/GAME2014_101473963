using UnityEngine;

// Factory pattern for bullet creation
public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;

    [Header("Bullet Template")]
    public GameObject bulletPrefab;

    void Awake()
    {
        Instance = this;
    }

    // Factory method to construct and configure a new bullet instance
    public GameObject CreateBullet()
    {
        GameObject bullet;

        if (bulletPrefab != null)
        {
            bullet = Instantiate(bulletPrefab);
        }
        else
        {
            // Procedural fallback if no prefab is assigned in the Inspector
            bullet = new GameObject("Bullet");
            SpriteRenderer sr = bullet.AddComponent<SpriteRenderer>();
            sr.color = Color.yellow;
            CircleCollider2D col = bullet.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            Rigidbody2D rb = bullet.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            bullet.AddComponent<Bullet>();
        }

        bullet.SetActive(false);
        return bullet;
    }
}
