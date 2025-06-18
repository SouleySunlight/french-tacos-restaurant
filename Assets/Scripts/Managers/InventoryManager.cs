using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private List<Ingredient> allIngredients = new();
    [HideInInspector] public List<Ingredient> UnlockedIngredients { get; private set; } = new();
    [HideInInspector] public int Popularity { get; private set; } = new();
    private Dictionary<string, InventorySlot> inventory = new();
    private Dictionary<string, InventorySlot> unprocessedInventory = new();

    private int processedIngredientMaxAmount;
    private int unprocessedIngredientMaxAmount;

    public string GetStockString(Ingredient ingredient)
    {
        return "(" + inventory[ingredient.id].currentAmount + "/" + processedIngredientMaxAmount + ")";
    }

    public string GetUnprocessedStockString(Ingredient ingredient)
    {
        if (ingredient.NeedProcessing())
        {
            return "(" + unprocessedInventory[ingredient.id].currentAmount + "/" + unprocessedIngredientMaxAmount + ")";
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
        return inventory[ingredient.id].currentAmount < processedIngredientMaxAmount;
    }
    public bool CanAddUnprocessedIngredient(Ingredient ingredient)
    {
        return unprocessedInventory[ingredient.id].currentAmount < unprocessedIngredientMaxAmount;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        inventory[ingredient.id].currentAmount += 1;
        OnProcessedIngredientAdded();
    }

    public void AddIngredient(Ingredient ingredient, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!CanAddIngredient(ingredient)) { return; }
            AddIngredient(ingredient);
        }
        OnProcessedIngredientAdded();
    }

    public void AddUnprocessedIngredient(Ingredient ingredient)
    {
        unprocessedInventory[ingredient.id].currentAmount += 1;
        OnUnprocessedIngredientAdded();

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
                        ingredient.NeedProcessing() ? 0 : GlobalConstant.DEFAULT_INGREDIENT_AMOUNT
                        );
                    UnlockedIngredients.Add(ingredient);
                }
            }
            LoadPopularity();
            return;
        }

        foreach (var slot in data.slots)
        {
            inventory[slot.ingredientID] = new InventorySlot(slot.currentAmount);
            var ingredientToAdd = allIngredients.Find(ingredient => slot.ingredientID == ingredient.id);
            if (!ingredientToAdd)
            {
                continue;
            }
            UnlockedIngredients.Add(ingredientToAdd);
            LoadPopularity();
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
                    unprocessedInventory[ingredient.id] = new InventorySlot(GlobalConstant.DEFAULT_INGREDIENT_AMOUNT);
                }
            }
            return;
        }

        foreach (var slot in data.slots)
        {
            unprocessedInventory[slot.ingredientID] = new InventorySlot(slot.currentAmount);
        }
    }

    public void LoadPopularity()
    {
        Popularity = 0;
        foreach (var ingredient in UnlockedIngredients)
        {
            Popularity += ingredient.popularity;
        }
        Debug.Log("Popularity loaded: " + Popularity);
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
        if (ingredient.NeedProcessing())
        {
            unprocessedInventory[ingredient.id] = new InventorySlot(GlobalConstant.DEFAULT_INGREDIENT_AMOUNT);
            inventory[ingredient.id] = new InventorySlot(0);
            UnlockedIngredients.Add(ingredient);
            Popularity += ingredient.popularity;
            return;
        }
        inventory[ingredient.id] = new InventorySlot(GlobalConstant.DEFAULT_INGREDIENT_AMOUNT);
        UnlockedIngredients.Add(ingredient);
        Popularity += ingredient.popularity;
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

    public void UpdateProcessedInventoryMaxAmount()
    {
        processedIngredientMaxAmount = GlobalConstant.DEFAULT_INGREDIENT_MAX_AMOUNT + (int)GameManager.Instance.UpgradeManager.GetEffect("INGREDIENT_DISPLAYER");
    }
    public void UpdateUnprocessedInventoryMaxAmount()
    {
        unprocessedIngredientMaxAmount = GlobalConstant.DEFAULT_INGREDIENT_MAX_AMOUNT + (int)GameManager.Instance.UpgradeManager.GetEffect("FRIDGE");
    }

    public void SetupInventoriesMaxAmount()
    {
        UpdateProcessedInventoryMaxAmount();
        UpdateUnprocessedInventoryMaxAmount();
    }

    public int GetUnprocessedIngredientQuantity(Ingredient ingredient)
    {
        return unprocessedInventory.ContainsKey(ingredient.id) ? unprocessedInventory[ingredient.id].currentAmount : 0;
    }
    public int GetProcessedIngredientQuantity(Ingredient ingredient)
    {
        return inventory.ContainsKey(ingredient.id) ? inventory[ingredient.id].currentAmount : 0;
    }

    public int GetProcessedIngredientMaxAmount()
    {
        return processedIngredientMaxAmount;
    }
    public int GetUnprocessedIngredientMaxAmount()
    {
        return unprocessedIngredientMaxAmount;
    }

    void OnProcessedIngredientAdded()
    {
        GameManager.Instance.TacosMakerManager.UpdateButtonsVisual();
    }

    void OnUnprocessedIngredientAdded()
    {
        GameManager.Instance.HotplateManager.UpdateButtonsVisual();
        GameManager.Instance.FryerManager.UpdateButtonsVisual();
    }
}