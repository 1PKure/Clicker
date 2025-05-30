using UnityEngine;
using UnityEngine.Advertisements;

public class RewardAdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {

    [SerializeField] private string _androidUnityId = "Rewarded_Android";
    [SerializeField] private string _iosUnityId = "Rewardedl_iOS";


    private static RewardAdsManager _instance;
    public static RewardAdsManager Instance {
        get {
            if (_instance == null) {
                _instance = FindFirstObjectByType<RewardAdsManager>();
            }
            return _instance;
        }
    }

    private string _unityId = null;
    private bool isLoaded;

    void Start() {
#if UNITY_ANDROID
        _unityId = _androidUnityId;
#elif UNITY_IOS
        _unityId = _iosUnityId;
#endif
    }

    internal void Initialize() {
        Advertisement.Load(_unityId, this);
    }

    public void ShowRewardedAd() {
        if (isLoaded) {
            BannersManager.Instance.HideBannerAd();
            Advertisement.Show(_unityId, this);
            isLoaded = false;
        }
        else {
            Debug.Log("No se cargo el rewarded AD");
        }
    }

    public void OnUnityAdsAdLoaded(string placementId) {
        isLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        Debug.Log($"Interstitial failed to load: {error} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        Debug.Log($"Interstitial ad failed to show: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) {
        Debug.Log("Interstitial ad started showing.");
    }

    public void OnUnityAdsShowClick(string placementId) {
        Debug.Log("Interstitial ad clicked.");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        if (_unityId.Equals(_unityId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED) {
            Debug.Log($"Rewarded ad completed: {showCompletionState}");
            BannersManager.Instance.ShowBannerAd();
            GameManager.Instance.AddReward();
            Initialize();
        }
    }
}
