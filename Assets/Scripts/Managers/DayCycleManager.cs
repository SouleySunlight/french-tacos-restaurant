using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    private int currentDay = 1;
    private static readonly float DAY_DURATION_IN_SECONDS = 5f;
    private float currentDayTimeElapsed = 0f;
    private bool isDayOver = false;
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
            FinishDay();
        }
    }

    public void StartNewDay()
    {
        isDayOver = false;
        currentDayTimeElapsed = 0f;
        dayCycleVisual.UpdateDayDisplay(currentDay);
    }

    private void FinishDay()
    {
        GameManager.Instance.PauseGame();
        dayOverModalVisuals.ShowDayOverModal();
        currentDay++;
        GameManager.Instance.SaveGame();
        isDayOver = true;
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