using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    private int currentDay = 1;
    private float dayDurationInSeconds = 10f;

    public void StartNewDay()
    {
        StartCoroutine(DayCoroutine());
    }

    private IEnumerator DayCoroutine()
    {
        yield return new WaitForSeconds(dayDurationInSeconds);
        currentDay++;
        Debug.Log(currentDay);
        StartNewDay();
    }
}