using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : MonoBehaviour, IGameService
{
    void OnEnable()
    {
        ServiceLocator.Current.Register(this);
    }

    void OnDisable()
    {
        ServiceLocator.Current.Unregister(this);
    }

    private int enemiesKill;
    public int EnemiesKill
    {
        get => enemiesKill;
        set
        {
            enemiesKill = value;
            OnEnemyKillChanged.Invoke(enemiesKill);
        }
    }

    public event Action<int> OnEnemyKillChanged;

    private int powerUpCollected;
    public int PowerUpCollected
    {
        get => powerUpCollected;
        set
        {
            powerUpCollected = value;
            OnPowerUpCollected.Invoke(powerUpCollected);
        }
    }

    public event Action<int> OnPowerUpCollected;
}
