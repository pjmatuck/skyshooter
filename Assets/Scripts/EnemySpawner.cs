using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnTime;

    int spawned;
    public int Spawned
    {
        get => spawned;
        set
        {
            spawned = value;
            if (OnObjectSpawned != null)
            {
                OnObjectSpawned(spawned);
            }
        }
    }

    int destroyed;
    public int Destroyed
    {
        get => destroyed;
        set
        {
            destroyed = value;
            if (OnObjectDestroyed != null)
            {
                OnObjectDestroyed(destroyed);
            }
        }
    }

    public event Action<int> OnObjectDestroyed;
    public event Action<int> OnObjectSpawned;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnTime);

        ServiceLocator.Current.Get<GameManager>().OnGameStateChanged +=
            OnGameStateChange;
    }

    void SpawnEnemy()
    {
        var prefab = Instantiate(enemyPrefab, 
            new Vector3(UnityEngine.Random.Range(-2f, 2f), transform.position.y, transform.position.z), 
            Quaternion.identity, 
            transform);
        Spawned++;
        prefab.GetComponent<EnemyBehavior>().OnDestruction += OnEnemyDestruction;
    }

    private void OnGameStateChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GAMEOVER)
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnemyDestruction()
    {
        Destroyed++;
    }
}
