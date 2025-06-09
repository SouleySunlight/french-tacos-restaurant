using System.Collections.Generic;

public class UpgradeSlot
{
    public BaseUpgrade upgrade;
    public int currentLevel;

    public UpgradeSlot(BaseUpgrade up, int level)
    {
        upgrade = up;
        currentLevel = level;
    }
}

[System.Serializable]
public class UpgradeSlotSaveData
{
    public string upgradeID;
    public int currentLevel;
}

[System.Serializable]
public class UpgradeSaveData
{
    public List<UpgradeSlotSaveData> slots = new();
}
