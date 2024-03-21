using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour, IGameService
{
    [SerializeField] TMP_Text gameOverLabel;
    [SerializeField] TMP_Text gameOverCounter;
    [SerializeField] TMP_Text enemyKillCount;
    [SerializeField] GameObject[] playerHearts;

    int gameOverCounterValue = 3;
    int killCount = 0;
    int playerHp = 3;

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
}
