using UnityEngine;
using Unity.Services.Core;
using Unity.Services.LevelPlay;
using System.Threading.Tasks;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private string interstitialPlacement;
    private LevelPlayInterstitialAd interstitialAd;
    private bool levelPlayInitialized = false;
    private string appKey;

    void Awake()
    {
#if UNITY_ANDROID
        interstitialPlacement = "9cxrknwvyyg50jfd";
        appKey = "236ea3815";
#elif UNITY_IOS
        interstitialPlacement = "9cxrknwvyyg50jfd";
        appKey = "2370ea025";
#endif
    }

    async void Start()
    {
        await InitializeUnityServices();
        RegisterLevelPlayEvents();
        LevelPlay.Init(appKey);
        //LevelPlay.SetMetaData("is_test_suite", "enable");
    }

    private void RegisterLevelPlayEvents()
    {
        LevelPlay.OnInitSuccess += (config) =>
        {
            Debug.Log("LevelPlay initialized successfully!");
            levelPlayInitialized = true;
            //LevelPlay.LaunchTestSuite();
            CreateInterstitialAd();
            LoadInterstitialAd();
        };

        LevelPlay.OnInitFailed += (error) =>
        {
            Debug.LogError($"LevelPlay initialization failed: {error}");
        };
    }

    private async Task InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services initialized.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
        }
    }

    private void CreateInterstitialAd()
    {
        if (string.IsNullOrEmpty(interstitialPlacement)) return;

        interstitialAd = new LevelPlayInterstitialAd(interstitialPlacement);
        interstitialAd.OnAdLoaded += (adInfo) => Debug.Log($"Interstitial ad loaded. + {adInfo}");
        interstitialAd.OnAdLoadFailed += (adError) => { Debug.LogError($"Failed to load interstitial ad: {adError}"); };

        interstitialAd.OnAdDisplayed += (adInfo) =>
        {
            Debug.Log($"Interstitial ad displayed: {adInfo}");
        };

        interstitialAd.OnAdDisplayFailed += (adError) =>
        {
            Debug.LogError($"Failed to display interstitial ad: {adError}");
        };

        interstitialAd.OnAdClicked += (adInfo) =>
        {
            Debug.Log($"Interstitial ad clicked: {adInfo}");
        };
        interstitialAd.OnAdClosed += (adInfo) =>
        {
            LoadInterstitialAd();
        };
    }

    public void LoadInterstitialAd()
    {
        if (!levelPlayInitialized)
        {
            Debug.LogWarning("Cannot load ad: LevelPlay not initialized yet.");
            return;
        }
        interstitialAd?.LoadAd();
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }
}