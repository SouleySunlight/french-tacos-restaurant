using System;
using System.Collections;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    private int currentDay = 1;
    private static readonly float DAY_DURATION_IN_SECONDS = 10f;
    private float currentDayTimeElapsed = 0f;
    public bool isDayOver { get; private set; } = false;
    private DayCycleVisual dayCycleVisual;
    private DayOverModalVisuals dayOverModalVisuals;
    private RatingModalVisual ratingModalVisual;
    [SerializeField] private AudioClip closeShopSound;
    [SerializeField] private AudioClip dayOverSound;

    private bool hasRateTheGame = false;
    private bool refuseRatingTheGame = false;
    private int ratingNumberOfTimeAsked = 0;
    private bool didShowRatingModalThisSession = false;
    private string reviewUrl = "youtube.com/watch?v=u4ecB57jFhI&list=RDu4ecB57jFhI&start_radio=1";
    private bool isRatingModalEnable = false;

    void Awake()
    {
        dayCycleVisual = FindFirstObjectByType<DayCycleVisual>(FindObjectsInactive.Include);
        dayOverModalVisuals = FindFirstObjectByType<DayOverModalVisuals>(FindObjectsInactive.Include);
        ratingModalVisual = FindFirstObjectByType<RatingModalVisual>(FindObjectsInactive.Include);

    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }
        if (isDayOver) { return; }

        currentDayTimeElapsed += Time.deltaTime;
        dayCycleVisual.UpdateDayCycleCompletionBar(currentDayTimeElapsed / DAY_DURATION_IN_SECONDS);

        if (currentDayTimeElapsed >= DAY_DURATION_IN_SECONDS)
        {
            GameManager.Instance.SoundManager.PlaySFX(closeShopSound);
            isDayOver = true;
            TryToFinishDay();
        }
    }

    public void StartNewDay()
    {
        isDayOver = false;
        currentDayTimeElapsed = 0f;
        dayCycleVisual.UpdateDayDisplay(currentDay);
        GameManager.Instance.OrdersManager.AddFirstOrderOfTheDay();
    }

    private void FinishDay()
    {
        GameManager.Instance.SoundManager.PlaySFX(dayOverSound);
        GameManager.Instance.PauseGame();
        GameManager.Instance.ResetViewForNewDay();
        dayOverModalVisuals.ShowDayOverModal();
        AdActionBetweenDay();
        currentDay++;
        GameManager.Instance.SaveGame();
    }

    public void TryToFinishDay()
    {
        if (GameManager.Instance.OrdersManager.GetCurrentOrdersCount() == 0)
        {
            FinishDay();
        }
    }


    public void SetupDayCycle()
    {
        dayCycleVisual.UpdateDayDisplay(currentDay);
        StartNewDay();
    }

    public void ToNextDay()
    {
        dayCycleVisual.UpdateDayDisplay(currentDay);
        GameManager.Instance.WalletManager.ResetDailyCount();
        GameManager.Instance.WorkersManager.RenewWorkers();
        GameManager.Instance.ResumeGame();
        dayOverModalVisuals.HideDayOverModal();

        StartNewDay();
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }
    public void SetCurrentDay(int day)
    {
        currentDay = day;
        dayCycleVisual.UpdateDayDisplay(currentDay);
    }

    public void DoubleEarnedGold()
    {
        var moneyEarnedThisDay = GameManager.Instance.WalletManager.moneyEarnedThisDay;
        GameManager.Instance.WalletManager.ReceiveMoney(moneyEarnedThisDay);
        ToNextDay();
    }

    void AdActionBetweenDay()
    {
        var shouldShowRatingModal =
            isRatingModalEnable &&
            GameManager.Instance.isFirebaseInit &&
            !hasRateTheGame &&
            !refuseRatingTheGame &&
            !didShowRatingModalThisSession &&
            currentDay >= 3 &&
            ratingNumberOfTimeAsked <= 3;

        if (shouldShowRatingModal)
        {
            ratingModalVisual.ShowModal();
            didShowRatingModalThisSession = true;
            ratingNumberOfTimeAsked++;
            return;
        }
        var shouldShowAd = UnityEngine.Random.Range(0f, 1f) <= 0.8f;
        if (!shouldShowAd) { return; }
        GameManager.Instance.AdsManager.ShowInterstitialAd();
    }

    public void RefuseRating()
    {
        refuseRatingTheGame = true;
    }

    public void LoadFirebaseData()
    {
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero)
            .ContinueWithOnMainThread(fetchTask =>
            {
                if (fetchTask.IsCompleted)
                {
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                        .ContinueWithOnMainThread(task =>
                        {
                            reviewUrl = Application.platform == RuntimePlatform.Android ?
                                FirebaseRemoteConfig.DefaultInstance.GetValue("review_url_android").StringValue :
                                FirebaseRemoteConfig.DefaultInstance.GetValue("review_url_ios").StringValue;
                            isRatingModalEnable = FirebaseRemoteConfig.DefaultInstance.GetValue("is_rating_modal_enable").BooleanValue;
                        });
                }
            });
    }
}