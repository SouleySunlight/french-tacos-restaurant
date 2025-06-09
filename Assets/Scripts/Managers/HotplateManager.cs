using System.Collections.Generic;
using UnityEngine;

public class HotplateManager : MonoBehaviour
{
    private HotplateVisuals hotplateVisuals;
    private List<Ingredient> cookingIngredients = new();
    private List<float> cookingTimes = new();
    private List<float> totalCookingTimes = new();


    void Awake()
    {
        hotplateVisuals = FindFirstObjectByType<HotplateVisuals>(FindObjectsInactive.Include);
        for (int i = 0; i < GlobalConstant.MAX_COOKING_INGREDIENTS; i++)
        {
            cookingIngredients.Add(null);
            cookingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
            totalCookingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
        }
    }
    void Update()
    {
        for (int i = 0; i < cookingTimes.Count; i++)
        {
            if (cookingTimes[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }

            if (cookingTimes[i] >= totalCookingTimes[i])
            {
                hotplateVisuals.OnIngredientCooked(i);
            }

            if (cookingTimes[i] >= totalCookingTimes[i] + cookingIngredients[i].wastingTimeOffset)
            {
                hotplateVisuals.OnIngredientBurnt(i);
            }

            cookingTimes[i] += Time.deltaTime;
            hotplateVisuals.UpdateTimer(i, cookingTimes[i] / totalCookingTimes[i]);


        }
    }

    public void SetupIngredients()
    {
        hotplateVisuals.SetupIngredients(GetIngredientsToCook());
    }

    public void AddAvailableIngredient(Ingredient ingredient)
    {
        if (ingredient.category == IngredientCategoryEnum.MEAT)
        {
            hotplateVisuals.AddAvailableIngredient(ingredient);
        }
    }

    List<Ingredient> GetIngredientsToCook()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.MEAT);
    }

    public void CookIngredients(Ingredient ingredient)
    {
        for (int i = 0; i < cookingIngredients.Count; i++)
        {
            if (cookingIngredients[i] != null)
            {
                continue;
            }
            if (!GameManager.Instance.InventoryManager.IsUnprocessedIngredientAvailable(ingredient))
            {
                return;
            }
            GameManager.Instance.InventoryManager.ConsumeUnprocessedIngredient(ingredient);
            cookingIngredients[i] = ingredient;
            cookingTimes[i] = 0;
            totalCookingTimes[i] = cookingIngredients[i].processingTime * GameManager.Instance.UpgradeManager.GetEffect("HOTPLATE");
            hotplateVisuals.CookIngredients(ingredient, i);
            return;
        }
        throw new NotEnoughSpaceException();

    }

    public void OnClickOnIngredient(int position)
    {
        var cookingTime = cookingTimes[position];
        var ingredient = cookingIngredients[position];

        if (cookingTime < totalCookingTimes[position])
        {
            return;
        }

        if (cookingTime > totalCookingTimes[position] + cookingIngredients[position].wastingTimeOffset)
        {
            RemoveIngredientFromCooking(position);
            return;
        }

        var ingredientToAdd = cookingIngredients[position];
        if (GameManager.Instance.InventoryManager.CanAddIngredient(ingredientToAdd))
        {
            GameManager.Instance.InventoryManager.AddIngredient(ingredient);
            RemoveIngredientFromCooking(position);
        }
    }

    void RemoveIngredientFromCooking(int position)
    {
        cookingIngredients[position] = null;
        cookingTimes[position] = GlobalConstant.UNUSED_TIME_VALUE;
        totalCookingTimes[position] = GlobalConstant.UNUSED_TIME_VALUE;
        hotplateVisuals.RemoveIngredientFromGrill(position);
    }
}
