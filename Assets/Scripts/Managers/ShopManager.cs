using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private ShopVisuals shopVisuals;

    void Awake()
    {
        shopVisuals = FindFirstObjectByType<ShopVisuals>(FindObjectsInactive.Include);
    }

    public void SetupShop()
    {
        var ingredientToBuy = GameManager.Instance.InventoryManager.GetIngredientsToUnlock();
        shopVisuals.SetupIngredientToBuy(ingredientToBuy);
    }

    public void UnlockIngredient(Ingredient ingredient)
    {
        GameManager.Instance.InventoryManager.UnlockIngredient(ingredient);
        shopVisuals.RemoveIngredientToBuy(ingredient);
    }
}