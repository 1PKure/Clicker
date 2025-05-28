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
    private bool _isRewardedReady = false;
    private int _rewardSeconds = 2;
    private bool _isInterstitialReady = false;

    public bool IsRewardedAdReady() => _isRewardedReady;
    public bool IsInterstitialAdReady() => _isInterstitialReady;

    void Start()
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
//#if UNITY_EDITOR || UNITY_ANDROID
        Advertisement.Initialize(_androidGameId, _testMode, this);
//#endif
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
        if (!_isInitialized || !_isInterstitialReady)
        {
            Debug.LogWarning("El Interstitial Ad no está listo todavía.");
            return;
        }

        Advertisement.Show(_androidInterstitialId, this);
        _isInterstitialReady = false;
    }

    public void LoadRewardedAd()
    {
        if (!_isInitialized) return;

        Advertisement.Load(_androidRewardedId, this);
    }

    private System.Action _onRewardComplete;

    public void ShowRewardedAd(System.Action onComplete)
    {
        if (!_isInitialized || !_isRewardedReady)
        {
            Debug.LogWarning("El Rewarded Ad no está listo todavía.");
            return;
        }

        _onRewardComplete = onComplete;
        Advertisement.Show(_androidRewardedId, this);
        _isRewardedReady = false;
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad cargado exitosamente: {placementId}");
        if (placementId == _androidRewardedId)
            _isRewardedReady = true;

        if (placementId == _androidInterstitialId)
            _isInterstitialReady = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error al cargar el AD {placementId}: {error} - {message}");
        if (placementId == _androidRewardedId)
            _isRewardedReady = false;

        if (placementId == _androidInterstitialId)
            _isInterstitialReady = false;
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