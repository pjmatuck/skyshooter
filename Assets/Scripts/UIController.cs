using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour, IGameService
{
    [SerializeField] TMP_Text gameOverLabel;
    [SerializeField] TMP_Text gameOverCounter;
    [SerializeField] TMP_Text enemyKillCount;
    [SerializeField] GameObject[] playerHearts;
    [SerializeField] GameObject startStageLabel;
    [SerializeField] TMP_Text levelName;
    [SerializeField] GameObject stageClearLabel;

    int gameOverCounterValue = 3;
    int killCount = 0;
    int playerHp = 3;

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

    public void IncreaseKillCount()
    {
        killCount++;
        enemyKillCount.text = killCount.ToString();
    }

    public void DecreaseHP()
    {
        playerHp--;
        playerHearts[playerHp].gameObject.SetActive(false);
    }

    public void RestoreKillCount()
    {
        killCount = 0;
        enemyKillCount.text = killCount.ToString();
    }

    public void RestoreHP()
    {
        playerHp = 3;
        foreach(var heart in playerHearts)
        {
            heart.gameObject.SetActive(true);
        }
    }

    public void RestoreUI()
    {
        RestoreHP();
        RestoreKillCount();
    }

    public void BlinkStartStageLabel(float blinkTime)
    {
        StartCoroutine(BlinkLabel(startStageLabel, blinkTime));
    }

    public void BlinkStageClearLabel(float blinkTime)
    {
        StartCoroutine(BlinkLabel(stageClearLabel, blinkTime));
    }

    IEnumerator BlinkLabel(GameObject label, float blinkTime)
    {
        float time = 0f;
        while(time < blinkTime)
        {
            var waitTime = blinkTime / 10;
            label.SetActive(!label.activeSelf);
            time += waitTime;
            yield return new WaitForSeconds(waitTime);
        }
        label.SetActive(false);
    }

    public void SetLevelName(string name)
    {
        levelName.text = $"Stage {name}";
    }

    private void OnEnable()
    {
        if (ServiceLocator.Current != null)
            ServiceLocator.Current.Register(this);
    }

    private void OnDisable()
    {
        if (ServiceLocator.Current != null)
            ServiceLocator.Current.Unregister(this);
    }
}
