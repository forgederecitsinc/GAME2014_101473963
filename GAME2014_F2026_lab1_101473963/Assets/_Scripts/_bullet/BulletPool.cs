using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [Header("Pool Configuration")]
    public int poolSize = 20;
    public BulletFactory bulletFactory;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (bulletFactory == null)
        {
            bulletFactory = BulletFactory.Instance ?? FindObjectOfType<BulletFactory>();
            if (bulletFactory == null)
            {
                bulletFactory = gameObject.AddComponent<BulletFactory>();
            }
        }

        // Pre-warm the pool using the BulletFactory
        for (int i = 0; i < poolSize; i++)
        {
            if (bulletFactory != null)
            {
                GameObject bullet = bulletFactory.CreateBullet();
                if (bullet != null)
                {
                    bullet.transform.SetParent(transform);
                    bullet.SetActive(false);
                    pool.Enqueue(bullet);
                }
            }
        }
    }

    // Pulls a bullet from the queue, positions it, and activates it
    public GameObject GetBullet(Vector3 position)
    {
        GameObject bullet = null;

        if (pool.Count > 0)
        {
            bullet = pool.Dequeue();
        }
        else if (bulletFactory != null)
        {
            // Expand pool on demand using factory if exhausted
            bullet = bulletFactory.CreateBullet();
            if (bullet != null)
            {
                bullet.transform.SetParent(transform);
            }
        }

        if (bullet != null)
        {
            bullet.transform.position = position;
            bullet.SetActive(true);
        }

        return bullet;
    }

    // Returns a spent bullet back into the queue
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}
