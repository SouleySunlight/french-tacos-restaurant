using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    private int currentDay = 1;
    private static readonly float DAY_DURATION_IN_SECONDS = 10f;
    private float currentDayTimeElapsed = 0f;
    public bool isDayOver { get; private set; } = false;
    private DayCycleVisual dayCycleVisual;
    private DayOverModalVisuals dayOverModalVisuals;

    void Awake()
    {
        dayCycleVisual = FindFirstObjectByType<DayCycleVisual>(FindObjectsInactive.Include);
        dayOverModalVisuals = FindFirstObjectByType<DayOverModalVisuals>(FindObjectsInactive.Include);
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }
        if (isDayOver) { return; }

        currentDayTimeElapsed += Time.deltaTime;
        dayCycleVisual.UpdateDayCycleCompletionBar(currentDayTimeElapsed / DAY_DURATION_IN_SECONDS);

        if (currentDayTimeElapsed >= DAY_DURATION_IN_SECONDS)
        {
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
        GameManager.Instance.PauseGame();
        GameManager.Instance.ResetViewForNewDay();
        dayOverModalVisuals.ShowDayOverModal();
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
}