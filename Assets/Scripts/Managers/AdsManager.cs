using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener {

    [Header("Game Ids")]  
    [SerializeField] private string androidGameId = "GameId_Android";
    [SerializeField] private string iosGameId = "GameId_iOS";


    private string gameId;
    private bool testMode = true;

    void Awake() {
#if UNITY_ANDROID
        gameId = androidGameId;
        testMode = true;
#elif UNITY_IOS
        gameId = iosGameId;
        testMode = false;
#elif UNITI_EDITOR
        gameId = androidGameId;
        testMode = true;
#endif

#if UNITY_ANDROID || UNITY_IOS
        if (!Advertisement.isInitialized && Advertisement.isSupported)
            Advertisement.Initialize(gameId, testMode, this);
#endif
    }

    public void OnInitializationComplete() {
        Debug.Log("Unity Ads Initialization Complete");

        BannersManager.Instance.Initialize();
        InterstitialManager.Instance.Initialize();
        RewardAdsManager.Instance.Initialize();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }
}
