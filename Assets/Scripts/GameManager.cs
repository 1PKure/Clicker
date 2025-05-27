using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text clickCountText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private Button clickButton;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private Button rewardedAdButton;


    private int clickCount = 0;
    private float gameTime = 10f;
    private bool isGameActive = false;
    private int highScore = 0;

    void Start()
    {
#if UNITY_WEBGL
        if (rewardedAdButton != null)
            rewardedAdButton.SetActive(false);
#endif
        rewardedAdButton.gameObject.SetActive(false);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High score: " + highScore;

        UpdateTimerDisplay();

        clickButton.interactable = true;
    }

    public void StartGame()
    {
        clickCount = 0;
        clickCountText.text = "00 clicks";

        gameTime = 10f;

        if (PlayerPrefs.HasKey("ExtraTimeReward"))
        {
            int extraTime = PlayerPrefs.GetInt("ExtraTimeReward");
            gameTime += extraTime;
            PlayerPrefs.DeleteKey("ExtraTimeReward");
        }

        isGameActive = true;
        rewardedAdButton.gameObject.SetActive(false);

        StartCoroutine(CountdownTimer());
    }


    public void ButtonClicked()
    {
        if (!isGameActive)
        {
            StartGame();
            return;
        }

        clickCount++;
        clickCountText.text = clickCount.ToString("00") + " clicks";
    }

    private IEnumerator CountdownTimer()
    {
        while (gameTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            gameTime -= 0.1f;
            gameTime = Mathf.Max(gameTime, 0);
            UpdateTimerDisplay();
        }

        EndGame();
    }

    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(gameTime);
        int milliseconds = Mathf.FloorToInt((gameTime - seconds) * 100);
        timerText.text = "Time: " + string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }

    private void EndGame()
    {
#if UNITY_ANDROID
        if (AdManager.Instance != null && AdManager.Instance.IsRewardedAdReady())
                rewardedAdButton.gameObject.SetActive(true);
            else
                rewardedAdButton.gameObject.SetActive(false);
#endif
        isGameActive = false;
        clickButton.interactable = true;

        if (clickCount > highScore)
        {
            highScore = clickCount;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = "High score: " + highScore;
            
            if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                AdManager.Instance.ShowInterstitial();
#endif
            }
        }
        else
        {
            rewardedAdButton.gameObject.SetActive(true);
        }
#if UNITY_ANDROID

        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ScheduleReturnNotification();
#endif
    }
#if UNITY_ANDROID
    public void RequestReward()
    {

        if (AdManager.Instance != null)
            AdManager.Instance.ShowRewardedAd(OnRewardGranted);
    }
#endif

    private void OnRewardGranted()
    {
        StartGame();
    }
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}