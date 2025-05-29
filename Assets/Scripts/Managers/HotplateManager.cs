using System.Collections.Generic;
using UnityEngine;

public class HotplateManager : MonoBehaviour
{
    private HotplateVisuals hotplateVisuals;

    void Awake()
    {
        hotplateVisuals = FindFirstObjectByType<HotplateVisuals>(FindObjectsInactive.Include);
    }

    void Start()
    {
        hotplateVisuals.SetupIngredients(GetIngredientsToCook());
    }

    List<Ingredient> GetIngredientsToCook()
    {
        return GameManager.Instance.AvailableIngredients.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.MEAT);
    }
}
