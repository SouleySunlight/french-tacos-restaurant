using System.Collections.Generic;

public class InventorySlot
{
    public int currentAmount;
    public int maxAmount;

    public InventorySlot()
    {
        currentAmount = GlobalConstant.DEFAULT_INGREDIENT_AMOUNT;
        maxAmount = GlobalConstant.DEFAULT_INGREDIENT_MAX_AMOUNT;
    }

    public InventorySlot(int current, int max)
    {
        currentAmount = current;
        maxAmount = max;
    }
}
[System.Serializable]
public class InventorySlotSaveData
{
    public string ingredientID;
    public int currentAmount;
    public int maxAmount;
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventorySlotSaveData> slots = new();
}