using System.Collections.Generic;
using UnityEngine;

public class UpgradeVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject upgradeButtonPrefab;
    [SerializeField] private Transform firstButtonPosition;

    private List<GameObject> buttons = new();

    public void SetupUpgrades(List<BaseUpgrade> upgrades, Dictionary<string, int> currentLevels)
    {
        buttons.Clear();
        foreach (BaseUpgrade upgrade in upgrades)
        {
            var buttonPrefab = Instantiate(upgradeButtonPrefab, firstButtonPosition.position, Quaternion.identity, firstButtonPosition);
            buttonPrefab.GetComponent<UpgradeButtonDisplayer>().upgradeData = upgrade;
            buttonPrefab.GetComponent<UpgradeButtonDisplayer>().currentLevel = currentLevels[upgrade.id];

            buttons.Add(buttonPrefab);
        }
        UpdateVisual();
    }
    void UpdateVisual()
    {
        var index = 0;
        foreach (var button in buttons)
        {
            var buttonPosition = new Vector3(
                           firstButtonPosition.position.x,
                           firstButtonPosition.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * index,
                           firstButtonPosition.position.z
                       );
            button.GetComponent<RectTransform>().position = buttonPosition;
            index++;
        }
    }
}