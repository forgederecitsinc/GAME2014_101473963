using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 5f;

    public float bottomEdge = -10f;

    void Update()
    {
        // Move down
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // If it goes off screen, return it to the pool instead of destroying it
        if (transform.position.y <= bottomEdge)
        {
            ObjectPool.Instance.ReturnEnemy(gameObject);
        }
    }
}