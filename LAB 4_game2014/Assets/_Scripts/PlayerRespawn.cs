using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }

    public void SetCheckpoint(Vector3 newSpawnPosition)
    {
        spawnPosition = newSpawnPosition;
    }

    public void Respawn()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        transform.position = spawnPosition;
    }
}
