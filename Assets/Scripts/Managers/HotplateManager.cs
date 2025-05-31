using System.Collections.Generic;
using UnityEngine;

public class HotplateManager : MonoBehaviour
{
    private HotplateVisuals hotplateVisuals;
    private List<Ingredient> cookingIngredients = new();
    private List<float> cookingTimes = new();


    void Awake()
    {
        hotplateVisuals = FindFirstObjectByType<HotplateVisuals>(FindObjectsInactive.Include);
        for (int i = 0; i < GlobalConstant.MAX_COOKING_INGREDIENTS; i++)
        {
            cookingIngredients.Add(null);
            cookingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
        }
    }

    void Start()
    {
        hotplateVisuals.SetupIngredients(GetIngredientsToCook());
    }

    void Update()
    {
        for (int i = 0; i < cookingTimes.Count; i++)
        {
            if (cookingTimes[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }

            if (cookingTimes[i] >= cookingIngredients[i].processingTime)
            {
                hotplateVisuals.OnIngredientCooked(i);
                continue;
            }

            cookingTimes[i] += Time.deltaTime;
            hotplateVisuals.UpdateTimer(i, cookingTimes[i] / cookingIngredients[i].processingTime);


        }
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
            cookingTimes[i] = 0;
            return;
        }
        throw new NotEnoughSpaceException();

    }
}
