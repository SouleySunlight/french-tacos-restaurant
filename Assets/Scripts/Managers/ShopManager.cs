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
            shopVisuals.SetupIngredientToRefill(GameManager.Instance.InventoryManager.UnlockedIngredients);
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
    }
}