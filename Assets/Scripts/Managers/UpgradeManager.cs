using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] List<BaseUpgrade> upgradables = new();
    private Dictionary<string, int> upgrades = new();

    private UpgradeVisual upgradeVisual;

    void Awake()
    {
        upgradeVisual = FindAnyObjectByType<UpgradeVisual>(FindObjectsInactive.Include);
    }

    void Start()
    {
        foreach (var upgradable in upgradables)
        {
            upgrades[upgradable.id] = GlobalConstant.DEFAULT_UPGRADE_LEVEL;
        }
    }

    public void SetupUpgrades()
    {
        upgradeVisual.SetupUpgrades(upgradables, upgrades);
    }
}