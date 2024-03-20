using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour, IGameService
{
    [SerializeField] TMP_Text gameOverLabel;
    [SerializeField] TMP_Text gameOverCounter;

    int gameOverCounterValue = 3;

    private void Awake()
    {
        ServiceLocator.Current.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Current.Unregister(this);
    }

    public void GameOver()
    {
        gameOverLabel.gameObject.SetActive(true);
        gameOverCounter.gameObject.SetActive(true);

        InvokeRepeating(nameof(StartGameOverCounter), 0f, 1f);
    }

    private void StartGameOverCounter()
    {
        if(gameOverCounterValue < 1) 
        {
            SceneManager.LoadScene("StartScene");
        }

        gameOverCounter.text = gameOverCounterValue.ToString();
        gameOverCounterValue--;

    }
}
