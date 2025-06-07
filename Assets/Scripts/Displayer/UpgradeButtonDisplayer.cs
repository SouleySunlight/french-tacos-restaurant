using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeButtonDisplayer : MonoBehaviour
{
    public BaseUpgrade upgradeData;
    public int currentLevel;

    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        buttonText.text = upgradeData.id + " <br> LVL: " + currentLevel + "/" + upgradeData.maxLevel + "(" + upgradeData.GetCostAtLevel(currentLevel) + " € )";
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}