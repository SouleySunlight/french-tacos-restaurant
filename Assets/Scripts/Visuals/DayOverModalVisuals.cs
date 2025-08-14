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
        moneyEarnedText.text = MoneyUtils.FormatAmount(GameManager.Instance.WalletManager.moneyEarnedThisDay);
        moneySpentText.text = MoneyUtils.FormatAmount(GameManager.Instance.WalletManager.GetMoneySpentThisDay());
        benefitsText.text = MoneyUtils.FormatAmount((GameManager.Instance.WalletManager.moneyEarnedThisDay - GameManager.Instance.WalletManager.GetMoneySpentThisDay()));

        ingredientsMoneySpentText.text = MoneyUtils.FormatAmount(GameManager.Instance.WalletManager.moneySpentOnIngredientsThisDay);
        upgradesMoneySpentText.text = MoneyUtils.FormatAmount(GameManager.Instance.WalletManager.moneySpentOnUpgradeThisDay);
        workersMoneySpentText.text = MoneyUtils.FormatAmount(GameManager.Instance.WalletManager.moneySpentOnWorkersThisDay);

        dayOverModal.SetActive(true);
    }

    void HideDayOverModal()
    {
        dayOverModal.SetActive(false);
    }

}