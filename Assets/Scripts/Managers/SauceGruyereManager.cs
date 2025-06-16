using System.Collections.Generic;
using UnityEngine;

public class SauceGruyereManager : MonoBehaviour
{
    private SauceGruyereVisual sauceGruyereVisual;

    void Awake()
    {
        sauceGruyereVisual = FindFirstObjectByType<SauceGruyereVisual>(FindObjectsInactive.Include);
    }

    public void SetupIngredients()
    {
        sauceGruyereVisual.SetupIngredients(GetSauceGruyereComponent());
    }

    List<Ingredient> GetSauceGruyereComponent()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll((ingredient) => ingredient.category == IngredientCategoryEnum.SAUCE_GRUYERE_INGREDIENT);
    }


}