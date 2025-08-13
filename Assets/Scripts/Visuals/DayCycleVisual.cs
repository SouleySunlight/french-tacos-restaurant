using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private GameObject dayCycleCompletionBar;
    [SerializeField] private GameObject dayOverModal;
    [SerializeField] private TMP_Text modalTitle;
    [SerializeField] private Button nextDayButton;
    [SerializeField] private TMP_Text moneyEarnedText;
    [SerializeField] private TMP_Text moneySpendedText;
    [SerializeField] private TMP_Text benefitsText;


    void Start()
    {
        nextDayButton.onClick.AddListener(() => OnClickOnNextDay());
    }

    public void OnDayOver(int dayOver, float moneyEarned, float moneySpend)
    {
        dayOverModal.SetActive(true);
        modalTitle.text = "Day " + dayOver + " is over.";
        moneyEarnedText.text = "Money earned today: " + moneyEarned + " €";
        moneySpendedText.text = "Money spend today: " + moneySpend + " €";
        benefitsText.text = "Benefits: " + (moneyEarned - moneySpend) + " €";

        nextDayButton.GetComponentInChildren<TMP_Text>().text = "To day " + (dayOver + 1);

    }

    public void UpdateDayDisplay(int currentDay)
    {
        dayText.text = currentDay.ToString();
    }
    public void UpdateDayCycleCompletionBar(float completionPercentage)
    {
        dayCycleCompletionBar.GetComponent<RoundedCompletionBarDisplayer>().UpdateTimer(completionPercentage);
    }
    void OnClickOnNextDay()
    {
        dayOverModal.SetActive(false);
        GameManager.Instance.DayCycleManager.ToNextDay();
    }
}