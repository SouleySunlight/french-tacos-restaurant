using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private List<Ingredient> allIngredients = new();
    [HideInInspector] public List<Ingredient> UnlockedIngredients { get; private set; } = new();
    private Dictionary<string, InventorySlot> inventory = new();
    private Dictionary<string, InventorySlot> unprocessedInventory = new();


    public string GetStockString(Ingredient ingredient)
    {
        return "(" + inventory[ingredient.id].currentAmount + "/" + inventory[ingredient.id].maxAmount + ")";
    }

    public string GetUnprocessedStockString(Ingredient ingredient)
    {
        if (ingredient.NeedProcessing())
        {
            return "(" + unprocessedInventory[ingredient.id].currentAmount + "/" + unprocessedInventory[ingredient.id].maxAmount + ")";
        }
        return GetStockString(ingredient);
    }

    public bool IsIngredientAvailable(Ingredient ingredient)
    {
        return inventory[ingredient.id].currentAmount > 0;
    }
    public bool IsUnprocessedIngredientAvailable(Ingredient ingredient)
    {
        return unprocessedInventory[ingredient.id].currentAmount > 0;
    }

    public void ConsumeUnprocessedIngredient(Ingredient ingredient)
    {
        if (unprocessedInventory[ingredient.id].currentAmount <= 0) { return; }
        unprocessedInventory[ingredient.id].currentAmount -= 1;
    }
    public void ConsumeIngredient(Ingredient ingredient)
    {
        if (inventory[ingredient.id].currentAmount <= 0) { return; }
        inventory[ingredient.id].currentAmount -= 1;
    }

    public bool CanAddIngredient(Ingredient ingredient)
    {
        return inventory[ingredient.id].currentAmount < inventory[ingredient.id].maxAmount;
    }
    public bool CanAddUnprocessedIngredient(Ingredient ingredient)
    {
        return unprocessedInventory[ingredient.id].currentAmount < unprocessedInventory[ingredient.id].maxAmount;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        inventory[ingredient.id].currentAmount += 1;
    }

    public void AddUnprocessedIngredient(Ingredient ingredient)
    {
        unprocessedInventory[ingredient.id].currentAmount += 1;
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

    public InventorySaveData GetUnprocessedInventorySaveData()
    {
        var data = new InventorySaveData();
        foreach (var pair in unprocessedInventory)
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

        if (data.slots.Count == 0)
        {
            foreach (var ingredient in allIngredients)
            {
                if (ingredient.isUnlockedFromTheBeginning)
                {
                    inventory[ingredient.id] = new InventorySlot(
                        ingredient.NeedProcessing() ? 0 : GlobalConstant.DEFAULT_INGREDIENT_AMOUNT,
                        GlobalConstant.DEFAULT_INGREDIENT_MAX_AMOUNT
                        );
                    UnlockedIngredients.Add(ingredient);
                }
            }
            return;
        }

        foreach (var slot in data.slots)
        {
            inventory[slot.ingredientID] = new InventorySlot(slot.currentAmount, slot.maxAmount);
            var ingredientToAdd = allIngredients.Find(ingredient => slot.ingredientID == ingredient.id);
            if (!ingredientToAdd)
            {
                continue;
            }
            UnlockedIngredients.Add(ingredientToAdd);
        }
    }

    public void LoadUnprocessedInventoryFromSaveData(InventorySaveData data)
    {
        unprocessedInventory.Clear();

        if (data.slots.Count == 0)
        {
            foreach (var ingredient in allIngredients)
            {
                if (ingredient.isUnlockedFromTheBeginning && ingredient.NeedProcessing())
                {
                    unprocessedInventory[ingredient.id] = new InventorySlot(GlobalConstant.DEFAULT_INGREDIENT_AMOUNT, GlobalConstant.DEFAULT_INGREDIENT_MAX_AMOUNT);
                }
            }
            return;
        }

        foreach (var slot in data.slots)
        {
            unprocessedInventory[slot.ingredientID] = new InventorySlot(slot.currentAmount, slot.maxAmount);
        }
    }

    public List<Ingredient> GetIngredientsToUnlock()
    {
        List<Ingredient> ingredientsToUnlock = new();

        foreach (var ingredient in allIngredients)
        {
            if (!UnlockedIngredients.Contains(ingredient))
            {
                ingredientsToUnlock.Add(ingredient);
            }
        }
        ingredientsToUnlock.Sort((x, y) => x.priceToUnlock.CompareTo(y.priceToUnlock));
        return ingredientsToUnlock;
    }
    public void UnlockIngredient(Ingredient ingredient)
    {
        inventory[ingredient.id] = new InventorySlot(GlobalConstant.DEFAULT_INGREDIENT_AMOUNT, GlobalConstant.DEFAULT_INGREDIENT_MAX_AMOUNT);
        UnlockedIngredients.Add(ingredient);
    }

    public void RefillIngredient(Ingredient ingredient)
    {
        if (ingredient.NeedProcessing())
        {
            if (!CanAddUnprocessedIngredient(ingredient)) { return; }
            AddUnprocessedIngredient(ingredient);
            return;
        }

        if (!CanAddIngredient(ingredient)) { return; }
        AddIngredient(ingredient);
    }
}