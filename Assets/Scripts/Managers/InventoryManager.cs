using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private List<Ingredient> allIngredients = new();
    [HideInInspector] public List<Ingredient> UnlockedIngredients { get; private set; } = new();
    private Dictionary<string, InventorySlot> inventory = new();
    private ShopVisuals shopVisuals;

    void Awake()
    {
        shopVisuals = FindFirstObjectByType<ShopVisuals>(FindObjectsInactive.Include);
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

        if (data.slots.Count == 0)
        {
            foreach (var ingredient in allIngredients)
            {
                if (ingredient.isUnlockedFromTheBeginning)
                {
                    inventory[ingredient.id] = new InventorySlot();
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

    public void SetupShop()
    {
        var ingredientToBuy = GetIngredientsToUnlock();
        shopVisuals.SetupIngredientToBuy(ingredientToBuy);
    }

    public void UnlockIngredient(Ingredient ingredient)
    {
        inventory[ingredient.id] = new InventorySlot();
        UnlockedIngredients.Add(ingredient);
        shopVisuals.RemoveIngredient(ingredient);
    }
}