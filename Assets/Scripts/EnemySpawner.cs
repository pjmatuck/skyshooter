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

    bool isSpawning = false;

    public event Action<int> OnObjectDestroyed;
    public event Action<int> OnObjectSpawned;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnTime);

        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged +=
            OnLevelStateChange;
    }

    void SpawnEnemy()
    {
        if (!isSpawning) return;

        var prefab = Instantiate(enemyPrefab, 
            new Vector3(UnityEngine.Random.Range(-2f, 2f), transform.position.y, transform.position.z), 
            Quaternion.identity, 
            transform);
        Spawned++;
        prefab.GetComponent<EnemyBehavior>().OnDestruction += OnEnemyDestruction;
    }

    private void OnLevelStateChange(LevelState state)
    {
        switch (state)
        {
            case LevelState.RUN:
                isSpawning = true;
                break;
            case LevelState.COMPLETE:
                isSpawning = false;
                break;
        }
    }

    void OnEnemyDestruction()
    {
        Destroyed++;
    }
}
