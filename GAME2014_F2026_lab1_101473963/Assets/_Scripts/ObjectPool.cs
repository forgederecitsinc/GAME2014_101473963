using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Makes this script easily accessible from anywhere
    public static ObjectPool Instance; 
    
    public GameObject enemyPrefab;
    public int poolSize = 10;
    
    // The queue holds our inactive enemies waiting to be spawned
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Spawn all enemies at the start and hide them
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // Pulls an enemy from the queue to use
    public GameObject GetEnemy()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null; 
    }

    // Puts the enemy back in the queue when it goes off screen
    public void ReturnEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
