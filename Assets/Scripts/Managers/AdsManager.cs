using UnityEngine;
using Unity.Services.Core;
using Unity.Services.LevelPlay;
using System.Threading.Tasks;

public class AdsManager : MonoBehaviour
{
    private string interstitialPlacement;
    private string rewardedPlacement;
    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedAd;
    private bool levelPlayInitialized = false;
    private string appKey;
    public RewardedAdTypeEnum currentRewardContext = RewardedAdTypeEnum.NONE;

    void Awake()
    {
#if UNITY_ANDROID
        interstitialPlacement = "9cxrknwvyyg50jfd";
        rewardedPlacement = "wxc1l5t428olt0fg";
        appKey = "236ea3815";
#elif UNITY_IOS
        interstitialPlacement = "lv4z7sw9inmc8fi2";
        rewardedPlacement = "wxc1l5t428olt0fg";
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

            CreateRewardedAd();
            LoadRewardedAd();
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

    private void CreateRewardedAd()
    {
        if (string.IsNullOrEmpty(rewardedPlacement)) return;

        rewardedAd = new LevelPlayRewardedAd(rewardedPlacement);

        rewardedAd.OnAdLoaded += (adInfo) => Debug.Log($"[Rewarded] Loaded: {adInfo}");
        rewardedAd.OnAdLoadFailed += (adError) => Debug.LogError($"[Rewarded] Load failed: {adError}");
        rewardedAd.OnAdDisplayed += (adInfo) => Debug.Log($"[Rewarded] Displayed: {adInfo}");
        rewardedAd.OnAdDisplayFailed += (adError) => Debug.LogError($"[Rewarded] Display failed: {adError}");
        rewardedAd.OnAdClicked += (adInfo) => Debug.Log($"[Rewarded] Clicked: {adInfo}");
        rewardedAd.OnAdClosed += (adInfo) =>
        {
            LoadRewardedAd();
        };
        rewardedAd.OnAdRewarded += (adInfo, reward) =>
        {
            GrantRewardToPlayer();
        };
    }

    public void LoadRewardedAd()
    {
        rewardedAd?.LoadAd();
    }

    public void ShowRewardedAd(RewardedAdTypeEnum context)
    {
        currentRewardContext = context;

        if (rewardedAd != null && rewardedAd.IsAdReady())
        {
            rewardedAd.ShowAd();
        }
        else
        {
            Debug.Log("[Rewarded] Not ready yet.");
        }
    }

    private void GrantRewardToPlayer()
    {
        {
            switch (currentRewardContext)
            {
                case RewardedAdTypeEnum.DOUBLE_GOLD:
                    GameManager.Instance.DayCycleManager.DoubleEarnedGold();
                    break;
                case RewardedAdTypeEnum.RETRIEVE_MINIMAL_MONEY:
                    GameManager.Instance.WalletManager.RetriveMinimalGold();
                    break;
                case RewardedAdTypeEnum.UNLOCK_WORKER:
                    GameManager.Instance.WorkersManager.HireRandomWorker();
                    break;
                default:
                    break;
            }

            currentRewardContext = RewardedAdTypeEnum.NONE;
        }
    }
}