using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] List<BaseUpgrade> upgradables = new();
    private Dictionary<string, UpgradeSlot> upgrades = new();

    private UpgradeVisual upgradeVisual;

    void Awake()
    {
        upgradeVisual = FindAnyObjectByType<UpgradeVisual>(FindObjectsInactive.Include);
    }

    public void SetupUpgrades()
    {
        upgradeVisual.SetupUpgrades(upgrades.Values.ToList());
    }

    public void UpgradeElement(string id)
    {
        if (upgrades[id].currentLevel >= upgrades[id].upgrade.maxLevel) { return; }
        upgrades[id].currentLevel += 1;
        upgradeVisual.UpdateUpgradeButton(id);
        OnUpgradeElement(id);
    }

    void OnUpgradeElement(string id)
    {
        switch (id)
        {
            case "GRILL":
                GameManager.Instance.GrillManager.UpdateGrillingTime();
                return;
            case "FRIDGE":
                GameManager.Instance.InventoryManager.UpdateUnprocessedInventoryMaxAmount();
                return;
            case "INGREDIENT_DISPLAYER":
                GameManager.Instance.InventoryManager.UpdateProcessedInventoryMaxAmount();
                return;
            case "GRUYERE_POT":
                GameManager.Instance.SauceGruyereManager.UpdateCookingTime();
                return;
            default:
                return;

        }
    }

    public float GetEffect(string id)
    {
        return upgrades[id].upgrade.GetEffect(upgrades[id].currentLevel);
    }

    public UpgradeSaveData GetInventorySaveData()
    {
        var data = new UpgradeSaveData();
        foreach (var pair in upgrades)
        {
            data.slots.Add(new UpgradeSlotSaveData
            {
                upgradeID = pair.Key,
                currentLevel = pair.Value.currentLevel,
            });
        }
        return data;
    }

    public void LoadUpgradesFromSaveData(UpgradeSaveData data)
    {
        upgrades.Clear();

        if (data.slots.Count == 0)
        {
            foreach (var upgradable in upgradables)
            {
                upgrades[upgradable.id] = new UpgradeSlot(upgradable, GlobalConstant.DEFAULT_UPGRADE_LEVEL);
            }
            return;
        }

        foreach (var slot in data.slots)
        {
            var upgradeToAdd = upgradables.Find(upgradable => upgradable.id == slot.upgradeID);
            upgrades[slot.upgradeID] = new UpgradeSlot(upgradeToAdd, slot.currentLevel);
        }
    }
}