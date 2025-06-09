using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    private int currentDay = 1;
    private float dayDurationInSeconds = 30f;
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
        dayCycleVisual.OnDayOver(currentDay, GameManager.Instance.WalletManager.moneyEarnedThisDay, GameManager.Instance.WalletManager.moneySpendThisDay);
        currentDay++;

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
        StartNewDay();
    }
}