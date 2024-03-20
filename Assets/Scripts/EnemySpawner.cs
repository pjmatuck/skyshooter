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

        ServiceLocator.Current.Get<GameManager>().OnGameStateChanged +=
            OnGameStateChange;
    }

    void SpawnEnemy()
    {
        Instantiate(enemy, 
            new Vector3(Random.Range(-2f, 2f), transform.position.y, transform.position.z), 
            Quaternion.identity, 
            transform);
    }

    private void OnGameStateChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GAMEOVER)
        {
            gameObject.SetActive(false);
        }
    }
}
