using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameService
{
    public enum GameState
    {
        RUNNING,
        GAMEOVER
    }

    public event Action<GameState> OnGameStateChanged;

    public GameState CurrentState = GameState.RUNNING;

    public void SetState(GameState state)
    {
        CurrentState = state;
        OnGameStateChanged(CurrentState);
    }

    private void Awake()
    {
        ServiceLocator.Current.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Current.Unregister(this);
    }
}
