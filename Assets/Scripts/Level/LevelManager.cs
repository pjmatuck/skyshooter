using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IGameService
{
    [SerializeField] List<string> scenes;

    int _currentScene = -1;
    public int CurrentScene
    {
        get => _currentScene;
        set
        {
            _currentScene = value;
            if(_currentScene == scenes.Count)
            {
                _currentScene = 0;
            }
        }
    }

    public event Action<LevelState> OnLevelStateChanged;

    AbstractLevelController _currentController;
    UIController _uiController;

    void Start()
    {
        _uiController = ServiceLocator.Current.Get<UIController>();
        
        if (CurrentScene == -1)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        _uiController.RestoreUI();
        CurrentScene++;
        SceneManager.LoadScene(scenes[_currentScene], LoadSceneMode.Additive);
    }

    void UnloadCurrentScene()
    {
        SceneManager.UnloadSceneAsync(scenes[_currentScene]);
    }

    public void RegisterController(AbstractLevelController controller)
    {
        _currentController = controller;
        _uiController.BlinkStageLabel(_currentController.StartingTime);
        _uiController.SetLevelName(_currentController.LevelName);
        _currentController.OnLevelStateChanged += OnLevelStateChange;
    }

    public void OnLevelStateChange(LevelState state)
    {
        if(state == LevelState.FINISH)
        {
            UnloadCurrentScene();
            LoadNextLevel();
        }

        OnLevelStateChanged?.Invoke(state);
    }

    private void OnEnable()
    {
        if(ServiceLocator.Current != null)
            ServiceLocator.Current.Register(this);
    }

    private void OnDisable()
    {
        if (ServiceLocator.Current != null)
            ServiceLocator.Current.Unregister(this);
    }
}
