using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    private int currentDay = 1;
    private float dayDurationInSeconds = 180f;
    private DayCycleVisual dayCycleVisual;

    void Awake()
    {
        dayCycleVisual = FindFirstObjectByType<DayCycleVisual>(FindObjectsInactive.Include);
    }

    public void StartNewDay()
    {
        StartCoroutine(DayCoroutine());
    }

    private IEnumerator DayCoroutine()
    {
        yield return new WaitForSeconds(dayDurationInSeconds);
        GameManager.Instance.PauseGame();
        dayCycleVisual.OnDayOver(currentDay, GameManager.Instance.WalletManager.moneyEarnedThisDay, GameManager.Instance.WalletManager.moneySpendThisDay);
        currentDay++;
        GameManager.Instance.SaveGame();

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