using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLevelController : MonoBehaviour
{
    [SerializeField] float startingTime;
    [SerializeField] string levelName;

    public float StartingTime => startingTime;
    public string LevelName => levelName;

    LevelState _levelState;
    public LevelState LevelState
    {
        get => _levelState;
        set
        {
            _levelState = value;
            OnLevelStateChanged?.Invoke(_levelState);
        }
    }

    public event Action<LevelState> OnLevelStateChanged;

    protected LevelManager _levelManager;

    void Awake()
    {
        _levelManager = ServiceLocator.Current.Get<LevelManager>();
    }
}

public enum LevelState
{
    SETUP,
    RUN,
    PAUSE,
    FINISH
}
