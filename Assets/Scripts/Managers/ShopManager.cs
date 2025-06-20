using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private ShopVisuals shopVisuals;

    public bool isInUnlockMode { get; private set; } = true;

    void Awake()
    {
        shopVisuals = FindFirstObjectByType<ShopVisuals>(FindObjectsInactive.Include);
    }

    public void ChangeView()
    {
        isInUnlockMode = !isInUnlockMode;

        if (isInUnlockMode)
        {
            var ingredientToBuy = GameManager.Instance.InventoryManager.GetIngredientsToUnlock();
            shopVisuals.SetupIngredientToBuy(ingredientToBuy);
            return;
        }
        else
        {
            shopVisuals.SetupIngredientToRefill(GetIngredientsToRefill());
        }


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

    public void RefillIngredient(Ingredient ingredient)
    {
        GameManager.Instance.InventoryManager.RefillIngredient(ingredient);
        shopVisuals.UpdateIngredientButtonVisual(ingredient);
    }

    public List<Ingredient> GetIngredientsToRefill()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll(ingredient => ingredient.canBePurshased);
    }
}