using System.Collections.Generic;

public class GameSaveData
{
    public float playerMoney = 10f;
    public int currentDay;
    public int numberOfTacosServed;
    public InventorySaveData processedIngredientInventorySaveData = new();
    public List<Ingredient> unlockedIngredients = new();
    public UpgradeSaveData upgradeSaveData = new();

}