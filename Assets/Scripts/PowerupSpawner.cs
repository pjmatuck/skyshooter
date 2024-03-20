using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] GameObject powerup;
    [SerializeField] float spawnTime;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUp), spawnTime, spawnTime);

        ServiceLocator.Current.Get<GameManager>().OnGameStateChanged +=
            OnGameStateChange;
    }

    void SpawnPowerUp()
    {
        Vector3 position = new Vector3(
            Random.Range(-2.5f, 2.5f),
            Random.Range(0f, 5f),
            0f);

        Instantiate(powerup, position, Quaternion.identity);
    }

    private void OnGameStateChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GAMEOVER)
        {
            gameObject.SetActive(false);
        }
    }
}
