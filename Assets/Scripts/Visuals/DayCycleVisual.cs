using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private GameObject dayCycleCompletionBar;

    public void UpdateDayDisplay(int currentDay)
    {
        dayText.text = currentDay.ToString();
    }
    public void UpdateDayCycleCompletionBar(float completionPercentage)
    {
        dayCycleCompletionBar.GetComponent<RoundedCompletionBarDisplayer>().UpdateTimer(completionPercentage);
    }
}