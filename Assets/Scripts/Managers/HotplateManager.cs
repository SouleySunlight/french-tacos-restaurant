using System.Collections.Generic;
using UnityEngine;

public class HotplateManager : MonoBehaviour
{
    private HotplateVisuals hotplateVisuals;
    private List<Ingredient> cookingIngredients = new();
    private readonly int MAX_COOKING_INGREDIENTS = 4;

    void Awake()
    {
        hotplateVisuals = FindFirstObjectByType<HotplateVisuals>(FindObjectsInactive.Include);
        for (int i = 0; i < MAX_COOKING_INGREDIENTS; i++)
        {
            cookingIngredients.Add(null);
        }
    }

    void Start()
    {
        hotplateVisuals.SetupIngredients(GetIngredientsToCook());
    }

    List<Ingredient> GetIngredientsToCook()
    {
        return GameManager.Instance.AvailableIngredients.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.MEAT);
    }

    public void CookIngredients(Ingredient ingredient)
    {
        for (int i = 0; i < cookingIngredients.Count; i++)
        {
            if (cookingIngredients[i] != null)
            {
                continue;
            }
            hotplateVisuals.CookIngredients(ingredient, i);
            cookingIngredients[i] = ingredient;
            return;
        }
        throw new NotEnoughSpaceException();

    }
}
