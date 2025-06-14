using System.Collections.Generic;
using UnityEngine;

public class FryerManager : MonoBehaviour
{
    private FryerVisual fryerVisuals;
    private List<Ingredient> fryingIngredients = new();
    private List<float> fryingTimes = new();
    private List<float> totalFryingTimes = new();
    private List<int> fryingQuantities = new();

    private static int BASKET_SIZE = 5;

    void Awake()
    {
        fryerVisuals = FindFirstObjectByType<FryerVisual>(FindObjectsInactive.Include);
        fryerVisuals.Setup();
        for (int i = 0; i < GlobalConstant.MAX_FRYING_INGREDIENTS; i++)
        {
            fryingIngredients.Add(null);
            fryingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
            totalFryingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
            fryingQuantities.Add(0);
        }
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }

        for (int i = 0; i < fryingTimes.Count; i++)
        {
            if (fryingTimes[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }

            if (fryingTimes[i] >= totalFryingTimes[i])
            {
                // fryerVisuals.OnIngredientCooked(i);
            }

            if (fryingTimes[i] >= totalFryingTimes[i] + fryingIngredients[i].wastingTimeOffset)
            {
                //fryerVisuals.OnIngredientBurnt(i);
            }

            fryingTimes[i] += Time.deltaTime;
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


    public void FryIngredients(Ingredient ingredient)
    {
        for (int i = 0; i < fryingIngredients.Count; i++)
        {
            if (fryingIngredients[i] != ingredient && fryingIngredients[i] != null)
            {
                continue;
            }
            if (fryingQuantities[i] >= BASKET_SIZE)
            {
                continue;
            }
            if (!GameManager.Instance.InventoryManager.IsUnprocessedIngredientAvailable(ingredient))
            {
                return;
            }
            GameManager.Instance.InventoryManager.ConsumeUnprocessedIngredient(ingredient);
            if (fryingIngredients[i] == null)
            {
                fryingIngredients[i] = ingredient;
                fryingTimes[i] = 0;
                totalFryingTimes[i] = fryingIngredients[i].processingTime;
            }
            fryingQuantities[i]++;
            fryerVisuals.FryIngredients(ingredient, i);
            return;
        }
        throw new NotEnoughSpaceException();

    }
}