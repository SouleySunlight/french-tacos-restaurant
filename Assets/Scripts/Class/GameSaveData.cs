using System.Collections.Generic;

public class GameSaveData
{
    public float playerMoney = 10f;
    public int currentDay;
    public int numberOfTacosServed = 0;
    public int maxNumberOfOrders = 3;
    public int tacosPrice = 5;
    public InventorySaveData processedIngredientInventorySaveData = new();
    public List<Ingredient> unlockedIngredients = new();
    public UpgradeSaveData upgradeSaveData = new();

}