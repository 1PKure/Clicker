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
    [SerializeField] private GameObject highscorePanel;


    private int clickCount = 0;
    private float gameTime = 10f;
    private bool isGameActive = false;
    private int highScore = 0;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();
            }
            return _instance;
        }
    }

    void Start()
    {
#if UNITY_WEBGL
        if (rewardedAdButton != null)
            rewardedAdButton.SetActive(false);
#endif
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High score: " + highScore;

        UpdateTimerDisplay();

        clickButton.interactable = true;
    }

    public void StartGame()
    {
        clickCount = 0;
        clickCountText.text = "00 clicks";

        if (PlayerPrefs.HasKey("ExtraTimeReward"))
        {
            int extraTime = PlayerPrefs.GetInt("ExtraTimeReward");
            gameTime += extraTime;
            PlayerPrefs.DeleteKey("ExtraTimeReward");
        }

        isGameActive = true;
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
        rewardedAdButton.gameObject.SetActive(false);
        while (gameTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            gameTime -= 0.1f;
            gameTime = Mathf.Max(gameTime, 0);
            UpdateTimerDisplay();
        }

        EndGame();
    }
    public void AddReward()
    {
        gameTime += 2f;
        UpdateTimerDisplay();
    }
    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(gameTime);
        int milliseconds = Mathf.FloorToInt((gameTime - seconds) * 100);
        timerText.text = "Time: " + string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }

    private void EndGame()
    {
        rewardedAdButton.gameObject.SetActive(true);
        isGameActive = false;
        clickButton.interactable = true;
        gameTime = 10f;
        UpdateTimerDisplay();

        if (clickCount < highScore)
        {
            InterstitialManager.Instance.ShowInterstitialAd();

        }
        else
        {
            highScore = clickCount;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = "High score: " + highScore;
            highscorePanel.SetActive(true);
        }
    }

    public void RequestReward()
    {
        RewardAdsManager.Instance.ShowRewardedAd();
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