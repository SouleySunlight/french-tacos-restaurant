using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeButtonDisplayer : MonoBehaviour
{
    public UpgradeSlot upgradeData;

    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        buttonText.text = upgradeData.upgrade.id + " <br> LVL: " + upgradeData.currentLevel + "/" + upgradeData.upgrade.maxLevel + "(" + upgradeData.upgrade.GetCostAtLevel(upgradeData.currentLevel) + " € )";
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}