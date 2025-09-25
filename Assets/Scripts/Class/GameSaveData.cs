using System.Collections.Generic;

public class GameSaveData
{
    public float playerMoney = 0;
    public int currentDay = 0;
    public int numberOfTacosServed = 0;
    public int maxNumberOfOrders = 3;
    public int tacosPrice = 5;
    public int maxIngredientNumber = 10;
    public InventorySaveData processedIngredientInventorySaveData = new();
    public List<string> unlockedIngredients = new();
    public UpgradeSaveData upgradeSaveData = new();

}