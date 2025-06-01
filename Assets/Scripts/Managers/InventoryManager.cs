using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private List<Ingredient> initialAvailableIngredients = new();
    [HideInInspector] public List<Ingredient> UnlockedIngredients { get; private set; }
    private Dictionary<Ingredient, InventorySlot> inventory = new();

    private void Awake()
    {
        UnlockedIngredients = initialAvailableIngredients;
        foreach (var ingredient in UnlockedIngredients)
        {
            inventory[ingredient] = new InventorySlot();
        }
    }

    public string GetStockString(Ingredient ingredient)
    {
        return "(" + inventory[ingredient].currentAmount + "/" + inventory[ingredient].maxAmount + ")";
    }

    public bool IsIngredientAvailable(Ingredient ingredient)
    {
        return inventory[ingredient].currentAmount > 0;
    }

    public void ConsumeIngredient(Ingredient ingredient)
    {
        inventory[ingredient].currentAmount -= 1;
    }
}