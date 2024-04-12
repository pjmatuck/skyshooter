using System;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] GameObject powerupPrefab;
    [SerializeField] float spawnTime;

    int spawned;
    public int Spawned
    {
        get => spawned;
        set
        {
            spawned = value;
            OnObjectSpawned(spawned);
        }
    }

    int collected;
    public int Collected
    {
        get => collected;
        set
        {
            collected = value;
            OnObjectCollected(collected);
        }
    }

    bool isSpawning;

    public event Action<int> OnObjectCollected;
    public event Action<int> OnObjectSpawned;

    Transform _pool;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUp), spawnTime, spawnTime);

        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged +=
            OnGameStateChange;

        _pool = ServiceLocator.Current.Get<PoolManager>().PoolTransform;
    }

    void SpawnPowerUp()
    {
        if (!isSpawning) return;

        Vector3 position = new Vector3(
            UnityEngine.Random.Range(-2.5f, 2.5f),
            UnityEngine.Random.Range(0f, 5f),
            0f);

        var powerUp = Instantiate(powerupPrefab, position, Quaternion.identity, _pool);
        powerUp.GetComponent<PowerupBehavior>().OnPowerupCollected += OnPowerUpCollected;
        spawned++;
    }

    private void OnGameStateChange(LevelState state)
    {
        if (state == LevelState.RUN)
            isSpawning = true;
    }

    public void OnPowerUpCollected()
    {
        collected++;
    }
}
