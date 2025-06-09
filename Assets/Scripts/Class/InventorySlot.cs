using System.Collections.Generic;

public class InventorySlot
{
    public int currentAmount;

    public InventorySlot(int current)
    {
        currentAmount = current;
    }
}
[System.Serializable]
public class InventorySlotSaveData
{
    public string ingredientID;
    public int currentAmount;
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventorySlotSaveData> slots = new();
}