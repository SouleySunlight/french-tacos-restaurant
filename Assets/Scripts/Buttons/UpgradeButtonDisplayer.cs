using TMPro;
using UnityEngine;

public class UpgradeButtonDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text percentText;
    public void UpdateVisual()
    {
        var currentView = PlayzoneVisual.currentView;

        if (currentView == ViewToShowEnum.TACOS_MAKER || currentView == ViewToShowEnum.CHECKOUT)
        {
            upgradeButton.SetActive(false);
            return;
        }
        var upgradeKey = GetViewUpgradeId(currentView);
        upgradeCostText.text = GameManager.Instance.UpgradeManager.GetUpgradeCost(upgradeKey).ToString();
        levelText.text = $"{GameManager.Instance.UpgradeManager.GetCurrentLevel(upgradeKey)}/{GameManager.Instance.UpgradeManager.GetMaxLevel(upgradeKey)}";
        percentText.text = $"{GameManager.Instance.UpgradeManager.GetSpeedfactor(upgradeKey) * 100}%";

        upgradeButton.SetActive(true);
    }

    string GetViewUpgradeId(ViewToShowEnum view)
    {
        return view switch
        {
            ViewToShowEnum.GRILL => "GRILL",
            ViewToShowEnum.HOTPLATE => "HOTPLATE",
            ViewToShowEnum.SAUCE_GRUYERE => "GRUYERE_POT",
            ViewToShowEnum.FRYER => "FRYER",
            _ => string.Empty,
        };
    }


}