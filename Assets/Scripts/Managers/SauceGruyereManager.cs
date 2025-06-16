using System.Collections.Generic;
using UnityEngine;

public class SauceGruyereManager : MonoBehaviour
{
    private SauceGruyereVisual sauceGruyereVisual;
    private List<Ingredient> sauceGruyereIngredients = new();

    private float cookingTime = GlobalConstant.UNUSED_TIME_VALUE;
    private float totalCookingTime = GlobalConstant.UNUSED_TIME_VALUE;

    void Awake()
    {
        sauceGruyereVisual = FindFirstObjectByType<SauceGruyereVisual>(FindObjectsInactive.Include);
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }


        if (cookingTime == GlobalConstant.UNUSED_TIME_VALUE) { return; }

        cookingTime += Time.deltaTime;
        sauceGruyereVisual.UpdateTimer(cookingTime / totalCookingTime);



    }

    public void SetupIngredients()
    {
        sauceGruyereVisual.SetupIngredients(GetSauceGruyereComponent());
    }

    List<Ingredient> GetSauceGruyereComponent()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll((ingredient) => ingredient.category == IngredientCategoryEnum.SAUCE_GRUYERE_INGREDIENT);
    }

    public void AddIngredientToSauceGruyere(Ingredient ingredient)
    {
        if (sauceGruyereIngredients.Contains(ingredient))
        {
            return;
        }
        sauceGruyereIngredients.Add(ingredient);
        sauceGruyereVisual.AddIngredientToSauceGruyere(ingredient);
        if (sauceGruyereIngredients.Count == GetSauceGruyereComponent().Count)
        {
            CookSauceGruyere();
        }
    }

    void CookSauceGruyere()
    {
        cookingTime = 0;
        totalCookingTime = GetSauceGruyereIngredient().processingTime;
    }

    Ingredient GetSauceGruyereIngredient()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.Find((ingredient) => ingredient.category == IngredientCategoryEnum.SAUCE_GRUYERE);
    }


}