using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IGameService
{
    [SerializeField] TMP_Text gameOverLabel;
    [SerializeField] TMP_Text gameOverCounter;
    [SerializeField] TMP_Text enemyKillCount;
    [SerializeField] GameObject[] playerHearts;
    [SerializeField] GameObject startStageLabel;
    [SerializeField] TMP_Text levelName;
    [SerializeField] GameObject stageClearLabel;
    [SerializeField] TMP_Text powerUpIndicator;
    [SerializeField] Slider shieldTimer;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float hitEffectDuration;

    [DllImport("__Internal")]
    private static extern void Vibrate(int ms);

    int gameOverCounterValue = 3;
    int killCount = 0;
    int playerHp = 3;
    int powerUp = 1;

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

    public void IncreasePowerUP()
    {
        powerUp++;
        powerUpIndicator.text = powerUp.ToString();
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

    public void RestorePowerUP()
    {
        powerUp = 1;
        powerUpIndicator.text = powerUp.ToString();
    }

    public void RestoreUI()
    {
        RestoreHP();
        RestoreKillCount();
        RestorePowerUP();
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

    internal void IncreaseHP()
    {
        if (playerHp == 3) return;
        playerHp++;
        playerHearts[playerHp-1].gameObject.SetActive(true);
    }

    public void StartShieldCoolDown(float time)
    {
        StartCoroutine(ShieldCooldown(time));
    }

    IEnumerator ShieldCooldown(float duration)
    {
        float time = 0;
        float tickRate = duration / 100;
        shieldTimer.value = 0;
        while(time < duration)
        {
            shieldTimer.value += (100/duration) * tickRate;
            time += tickRate;
            yield return new WaitForSeconds(tickRate);
        }
    }

    public void EnableHitEffect()
    {
        StartCoroutine(HitEffect());
    }

    IEnumerator HitEffect()
    {
        hitEffect.SetActive(true);
#if !UNITY_EDITOR && UNITY_WEBGL
        Vibrate(200);
#else
        Debug.Log("Simulate vibration");
#endif
        yield return new WaitForSeconds(hitEffectDuration);
        hitEffect.SetActive(false);
    }
}