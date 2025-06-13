using System.Collections.Generic;
using UnityEngine;

public class FryerManager : MonoBehaviour
{
    private FryerVisual fryerVisuals;
    private List<Ingredient> cookingIngredients = new();
    private List<float> cookingTimes = new();
    private List<float> totalCookingTimes = new();

    void Awake()
    {
        fryerVisuals = FindFirstObjectByType<FryerVisual>(FindObjectsInactive.Include);
        fryerVisuals.Setup();
        for (int i = 0; i < GlobalConstant.MAX_FRYING_INGREDIENTS; i++)
        {
            cookingIngredients.Add(null);
            cookingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
            totalCookingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
        }
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }

        for (int i = 0; i < cookingTimes.Count; i++)
        {
            if (cookingTimes[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }

            if (cookingTimes[i] >= totalCookingTimes[i])
            {
                // fryerVisuals.OnIngredientCooked(i);
            }

            if (cookingTimes[i] >= totalCookingTimes[i] + cookingIngredients[i].wastingTimeOffset)
            {
                //fryerVisuals.OnIngredientBurnt(i);
            }

            cookingTimes[i] += Time.deltaTime;
            // fryerVisuals.UpdateTimer(i, cookingTimes[i] / totalCookingTimes[i]);
        }
    }

    public void SetupIngredients()
    {
        fryerVisuals.SetupIngredients(GetIngredientsToCook());
    }

    List<Ingredient> GetIngredientsToCook()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.FRIES);
    }

    public void AddAvailableIngredient(Ingredient ingredient)
    {
        if (ingredient.category == IngredientCategoryEnum.FRIES)
        {
            fryerVisuals.AddAvailableIngredient(ingredient);
        }
    }
}