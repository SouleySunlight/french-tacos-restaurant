public class GameSaveData
{
    public int playerMoney;
    public int currentDay;
    public InventorySaveData inventorySaveData = new();
    public InventorySaveData unprocessedInventorySaveData = new();
    public UpgradeSaveData upgradeSaveData = new();

}