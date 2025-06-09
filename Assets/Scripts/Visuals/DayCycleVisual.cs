using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private GameObject dayOverModal;
    [SerializeField] private TMP_Text modalTitle;
    [SerializeField] private Button nextDayButton;
    void Start()
    {
        nextDayButton.onClick.AddListener(() => OnClickOnNextDay());
    }

    public void OnDayOver(int dayOver)
    {
        dayOverModal.SetActive(true);
        modalTitle.text = "Day " + dayOver + " is over.";
        nextDayButton.GetComponentInChildren<TMP_Text>().text = "To day " + (dayOver + 1);

    }

    public void UpdateDayDisplay(int currentDay)
    {
        dayText.text = "Day " + currentDay;
    }
    void OnClickOnNextDay()
    {
        dayOverModal.SetActive(false);
        GameManager.Instance.DayCycleManager.ToNextDay();
    }
}