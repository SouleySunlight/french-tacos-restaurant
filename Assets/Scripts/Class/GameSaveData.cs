public class GameSaveData
{
    public float playerMoney;
    public int currentDay;
    public InventorySaveData inventorySaveData = new();
    public InventorySaveData unprocessedInventorySaveData = new();
    public UpgradeSaveData upgradeSaveData = new();

}