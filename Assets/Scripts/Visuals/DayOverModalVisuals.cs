using TMPro;
using UnityEngine;

public class DayOverModalVisuals : MonoBehaviour
{
    [SerializeField] private GameObject dayOverModal;
    [SerializeField] private TMP_Text dayOverTitle;
    [SerializeField] private TMP_Text moneyEarnedText;
    [SerializeField] private TMP_Text moneySpentText;
    [SerializeField] private TMP_Text benefitsText;
    [SerializeField] private TMP_Text ingredientsMoneySpentText;
    [SerializeField] private TMP_Text upgradesMoneySpentText;
    [SerializeField] private TMP_Text workersMoneySpentText;

    public void ShowDayOverModal()
    {
        dayOverTitle.text = "Day " + GameManager.Instance.DayCycleManager.GetCurrentDay() + " Over";
        moneyEarnedText.text = GameManager.Instance.WalletManager.moneyEarnedThisDay.ToString();
        moneySpentText.text = GameManager.Instance.WalletManager.GetMoneySpentThisDay().ToString();
        benefitsText.text = (GameManager.Instance.WalletManager.moneyEarnedThisDay - GameManager.Instance.WalletManager.GetMoneySpentThisDay()).ToString();

        ingredientsMoneySpentText.text = GameManager.Instance.WalletManager.moneySpentOnIngredientsThisDay.ToString();
        upgradesMoneySpentText.text = GameManager.Instance.WalletManager.moneySpentOnUpgradeThisDay.ToString();
        workersMoneySpentText.text = GameManager.Instance.WalletManager.moneySpentOnWorkersThisDay.ToString();

        dayOverModal.SetActive(true);
    }

    void HideDayOverModal()
    {
        dayOverModal.SetActive(false);
    }

}