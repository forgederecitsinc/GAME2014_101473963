using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate = 2f; // Seconds between spawns
    private float timer = 0f;
    
    // Where enemies appear at the top
    public float spawnY = 10f; 
    public float minX = -2.5f; // Left screen edge
    public float maxX = 2.5f;  // Right screen edge

    void Update()
    {
        timer += Time.deltaTime;
        
        // When the timer hits the spawn rate, spawn an enemy and reset
        if (timer >= spawnRate)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        // Ask the pool for an enemy
        GameObject enemy = ObjectPool.Instance.GetEnemy();
        
        if (enemy != null)
        {
            // Pick a random X position at the top of the screen
            float randomX = Random.Range(minX, maxX);
            enemy.transform.position = new Vector3(randomX, spawnY, 0);
        }
    }
}