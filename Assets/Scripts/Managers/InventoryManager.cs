using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private List<Ingredient> allIngredients = new();
    [HideInInspector] public List<Ingredient> UnlockedIngredients { get; private set; } = new();
    [HideInInspector] public int Popularity { get; private set; } = new();
    private Dictionary<string, InventorySlot> processedIngredientInventory = new();

    private int processedIngredientMaxAmount;

    public string GetProcessedIngredientStockString(Ingredient ingredient)
    {
        return processedIngredientInventory[ingredient.id].currentAmount + " / " + processedIngredientMaxAmount;
    }

    public bool IsProcessedIngredientAvailable(Ingredient ingredient)
    {
        return processedIngredientInventory[ingredient.id].currentAmount > 0;
    }

    public bool IsUnprocessedIngredientAvailable(Ingredient ingredient)
    {
        return GameManager.Instance.WalletManager.HasEnoughMoney(ingredient.priceToRefill);
    }

    public bool IsIngredientAvailableForTacos(Ingredient ingredient)
    {
        if (ingredient.NeedProcessing())
        {
            return IsProcessedIngredientAvailable(ingredient);
        }
        return IsUnprocessedIngredientAvailable(ingredient);
    }
    public void ConsumeUnprocessedIngredient(Ingredient ingredient)
    {
        if (!GameManager.Instance.WalletManager.HasEnoughMoney(ingredient.priceToRefill)) { return; }
        GameManager.Instance.WalletManager.SpendMoney(ingredient.priceToRefill);
    }
    public void ConsumeProcessedIngredient(Ingredient ingredient)
    {
        if (processedIngredientInventory[ingredient.id].currentAmount <= 0) { return; }
        processedIngredientInventory[ingredient.id].currentAmount -= 1;
    }

    public void ConsumeIngredientForTacos(Ingredient ingredient)
    {
        if (ingredient.NeedProcessing())
        {
            ConsumeProcessedIngredient(ingredient);
        }
        else
        {
            ConsumeUnprocessedIngredient(ingredient);
        }
    }

    public bool CanAddProcessedIngredient(Ingredient ingredient)
    {
        return processedIngredientInventory[ingredient.id].currentAmount < processedIngredientMaxAmount;
    }

    public void AddProcessedIngredient(Ingredient ingredient)
    {
        processedIngredientInventory[ingredient.id].currentAmount += 1;
        OnProcessedIngredientAdded();
    }

    public void AddProcessedIngredients(Ingredient ingredient, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!CanAddProcessedIngredient(ingredient)) { return; }
            AddProcessedIngredient(ingredient);
        }
        OnProcessedIngredientAdded();
    }

    public InventorySaveData GetInventoryProcessedIngredientSaveData()
    {
        var data = new InventorySaveData();
        foreach (var pair in processedIngredientInventory)
        {
            data.slots.Add(new InventorySlotSaveData
            {
                ingredientID = pair.Key,
                currentAmount = pair.Value.currentAmount,
            });
        }
        return data;
    }

    public List<Ingredient> GetUnlockedIngredients()
    {
        return UnlockedIngredients;
    }

    public void LoadUnlockedIngredientsFromSaveData(List<Ingredient> ingredients)
    {
        UnlockedIngredients.Clear();
        if (ingredients.Count == 0)
        {
            foreach (var ingredient in allIngredients)
            {
                if (ingredient.isUnlockedFromTheBeginning)
                {
                    UnlockedIngredients.Add(ingredient);
                    if (ingredient.NeedProcessing())
                    {
                        processedIngredientInventory[ingredient.id] = new InventorySlot(0);

                    }
                }
            }
            return;
        }
        foreach (var ingredient in ingredients)
        {
            if (ingredient.isUnlockedFromTheBeginning)
            {
                UnlockedIngredients.Add(ingredient);
            }
        }
    }

    public void LoadProcessedInventoryFromSaveData(InventorySaveData data)
    {
        processedIngredientInventory.Clear();
        foreach (var slot in data.slots)
        {
            processedIngredientInventory[slot.ingredientID] = new InventorySlot(slot.currentAmount);
        }
    }

    public void LoadPopularity()
    {
        Popularity = 0;
        foreach (var ingredient in UnlockedIngredients)
        {
            Popularity += ingredient.popularity;
        }
    }

    public void LoadInventory(InventorySaveData data, List<Ingredient> unlockedIngredients)
    {
        LoadProcessedInventoryFromSaveData(data);
        LoadUnlockedIngredientsFromSaveData(unlockedIngredients);
        LoadPopularity();
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
        UnlockedIngredients.Add(ingredient);
        Popularity += ingredient.popularity;
        if (ingredient.NeedProcessing())
        {
            processedIngredientInventory[ingredient.id] = new InventorySlot(0);
        }
    }

    public void UpdateProcessedInventoryMaxAmount()
    {
        processedIngredientMaxAmount = GlobalConstant.DEFAULT_INGREDIENT_MAX_AMOUNT + (int)GameManager.Instance.UpgradeManager.GetEffect("INGREDIENT_DISPLAYER");
    }


    public void SetupInventoriesMaxAmount()
    {
        UpdateProcessedInventoryMaxAmount();
    }
    public int GetProcessedIngredientQuantity(Ingredient ingredient)
    {
        return processedIngredientInventory.ContainsKey(ingredient.id) ? processedIngredientInventory[ingredient.id].currentAmount : 0;
    }

    public int GetProcessedIngredientMaxAmount()
    {
        return processedIngredientMaxAmount;
    }

    void OnProcessedIngredientAdded()
    {
        GameManager.Instance.TacosMakerManager.UpdateButtonsVisualQuantity();
    }

}