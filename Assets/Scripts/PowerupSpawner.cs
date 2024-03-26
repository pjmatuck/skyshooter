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

    public event Action<int> OnObjectCollected;
    public event Action<int> OnObjectSpawned;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUp), spawnTime, spawnTime);

        ServiceLocator.Current.Get<GameManager>().OnGameStateChanged +=
            OnGameStateChange;
    }

    void SpawnPowerUp()
    {
        Vector3 position = new Vector3(
            UnityEngine.Random.Range(-2.5f, 2.5f),
            UnityEngine.Random.Range(0f, 5f),
            0f);

        var powerUp = Instantiate(powerupPrefab, position, Quaternion.identity);
        powerUp.GetComponent<PowerupBehavior>().OnPowerupCollected += OnPowerUpCollected;
        spawned++;
    }

    private void OnGameStateChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GAMEOVER)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPowerUpCollected()
    {
        collected++;
    }
}
