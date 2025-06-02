using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private List<Ingredient> initialAvailableIngredients = new();
    [HideInInspector] public List<Ingredient> UnlockedIngredients { get; private set; }
    private Dictionary<string, InventorySlot> inventory = new();

    private void Awake()
    {
        UnlockedIngredients = initialAvailableIngredients;
        foreach (var ingredient in UnlockedIngredients)
        {
            inventory[ingredient.id] = new InventorySlot();
        }
    }

    public string GetStockString(Ingredient ingredient)
    {
        return "(" + inventory[ingredient.id].currentAmount + "/" + inventory[ingredient.id].maxAmount + ")";
    }

    public bool IsIngredientAvailable(Ingredient ingredient)
    {
        return inventory[ingredient.id].currentAmount > 0;
    }

    public void ConsumeIngredient(Ingredient ingredient)
    {
        inventory[ingredient.id].currentAmount -= 1;
    }

    public bool CanAddIngredient(Ingredient ingredient)
    {
        return inventory[ingredient.id].currentAmount < inventory[ingredient.id].maxAmount;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        inventory[ingredient.id].currentAmount += 1;
    }

    public InventorySaveData GetInventorySaveData()
    {
        var data = new InventorySaveData();
        foreach (var pair in inventory)
        {
            data.slots.Add(new InventorySlotSaveData
            {
                ingredientID = pair.Key,
                currentAmount = pair.Value.currentAmount,
                maxAmount = pair.Value.maxAmount
            });
        }
        return data;
    }

    public void LoadInventoryFromSaveData(InventorySaveData data)
    {
        inventory.Clear();

        foreach (var slot in data.slots)
        {
            inventory[slot.ingredientID] = new InventorySlot(slot.currentAmount, slot.maxAmount);
        }
    }
}