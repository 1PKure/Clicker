using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance { get; private set; }

    [SerializeField] private string _androidGameId;
    [SerializeField] private bool _testMode = true;
    [SerializeField] private string _androidBannerId = "Banner_Android";
    [SerializeField] private string _androidInterstitialId = "Interstitial_Android";
    [SerializeField] private string _androidRewardedId = "Rewarded_Android";

    private bool _isInitialized = false;
    private int _rewardSeconds = 2;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeAds()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Advertisement.Initialize(_androidGameId, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        _isInitialized = true;
        Debug.Log("Unity Ads initialization complete.");

        // Load banner ad
        LoadBanner();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }

    public void LoadBanner()
    {
        if (!_isInitialized) return;

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(_androidBannerId);
        Advertisement.Banner.Show(_androidBannerId);
    }

    public void LoadInterstitial()
    {
        if (!_isInitialized) return;

        Advertisement.Load(_androidInterstitialId, this);
    }

    public void ShowInterstitial()
    {
        if (!_isInitialized) return;

        Advertisement.Show(_androidInterstitialId, this);
    }

    public void LoadRewardedAd()
    {
        if (!_isInitialized) return;

        Advertisement.Load(_androidRewardedId, this);
    }

    public void ShowRewardedAd()
    {
        if (!_isInitialized) return;

        Advertisement.Show(_androidRewardedId, this);
    }

    // Implement IUnityAdsLoadListener interface methods
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error loading Ad Unit {placementId}: {error} - {message}");
    }

    // Implement IUnityAdsShowListener interface methods
    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(_androidRewardedId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Reward the player
            PlayerPrefs.SetInt("ExtraTimeReward", _rewardSeconds);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error showing Ad Unit {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) { }
}