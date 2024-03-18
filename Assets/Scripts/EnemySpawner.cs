using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float spawnTime;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnTime);
    }

    void SpawnEnemy()
    {
        Instantiate(enemy, 
            new Vector3(Random.Range(-2f, 2f), transform.position.y, transform.position.z), 
            Quaternion.identity, 
            transform);
    }
}
