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

    void Start()
    {
        foreach (var upgradable in upgradables)
        {
            upgrades[upgradable.id] = new UpgradeSlot(upgradable, GlobalConstant.DEFAULT_UPGRADE_LEVEL);
        }
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
        if (id == "GRILL")
        {
            GameManager.Instance.GrillManager.UpdateGrillingTime();
        }
    }

    public float GetEffect(string id)
    {
        return upgrades[id].upgrade.GetEffect(upgrades[id].currentLevel);
    }
}