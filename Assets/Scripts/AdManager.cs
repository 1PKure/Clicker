#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance { get; private set; }

    [SerializeField] private string _androidGameId;
    [SerializeField] private bool _testMode = false;
    [SerializeField] private string _androidBannerId = "Banner_Android";
    [SerializeField] private string _androidInterstitialId = "Interstitial_Android";
    [SerializeField] private string _androidRewardedId = "Rewarded_Android";

    private bool _isInitialized = false;
    private int _rewardSeconds = 2;

    void Awake()
    {
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
#if UNITY_EDITOR || UNITY_ANDROID
        Advertisement.Initialize(_androidGameId, _testMode, this);
#endif
    }

    public void OnInitializationComplete()
    {
        _isInitialized = true;
        LoadBanner();
        LoadRewardedAd();
        LoadInterstitial();
        Debug.Log("Los ads se inicializaron correctamente.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads no se inicializo correctamente: {error} - {message}");
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

    private System.Action _onRewardComplete;

    public void ShowRewardedAd(System.Action onComplete)
    {

        _onRewardComplete = onComplete;
        Advertisement.Show(_androidRewardedId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad cargado exitosamente: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error al cargar el AD {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(_androidRewardedId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            PlayerPrefs.SetInt("ExtraTimeReward", _rewardSeconds);
            LoadRewardedAd();

            _onRewardComplete?.Invoke();
            _onRewardComplete = null;
        }
    }


    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error al mostrar el AD {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) { }
}
#endif