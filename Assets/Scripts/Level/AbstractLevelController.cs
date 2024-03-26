using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLevelController : MonoBehaviour
{
    LevelState _levelState;
    public LevelState LevelState
    {
        get => _levelState;
        set
        {
            _levelState = value;
            OnLevelStateChanged(_levelState);
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
    RUNNING,
    COMPLETE
}
