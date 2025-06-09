using System.Collections.Generic;
using UnityEngine;

public class UpgradeVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject upgradeButtonPrefab;
    [SerializeField] private Transform firstButtonPosition;

    private List<GameObject> buttons = new();

    public void SetupUpgrades(List<UpgradeSlot> upgrades)
    {
        buttons.Clear();
        foreach (UpgradeSlot upgrade in upgrades)
        {
            var buttonPrefab = Instantiate(upgradeButtonPrefab, firstButtonPosition.position, Quaternion.identity, firstButtonPosition);
            buttonPrefab.GetComponent<UpgradeButtonDisplayer>().upgradeData = upgrade;
            buttonPrefab.GetComponent<UpgradeButtonDisplayer>().AddListener(() => OnClickOnUpgradeButton(upgrade));


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

    public void UpdateUpgradeButton(string id)
    {
        buttons.Find(button => button.GetComponent<UpgradeButtonDisplayer>().upgradeData.upgrade.id == id).GetComponent<UpgradeButtonDisplayer>().UpdateVisual();
    }
    void OnClickOnUpgradeButton(UpgradeSlot upgrade)
    {
        GameManager.Instance.UpgradeElement(upgrade);
    }
}