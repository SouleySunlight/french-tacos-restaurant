using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    private int currentDay = 1;
    private float dayDurationInSeconds = 10f;
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
        dayCycleVisual.OnDayOver(currentDay);
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
        StartNewDay();
    }
}