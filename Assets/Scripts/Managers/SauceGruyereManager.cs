using System.Collections.Generic;
using UnityEngine;

public class SauceGruyereManager : MonoBehaviour
{
    private SauceGruyereVisual sauceGruyereVisual;
    private List<Ingredient> sauceGruyereIngredients = new();
    [SerializeField] private Ingredient sauceGruyere;


    private float cookingTime = GlobalConstant.UNUSED_TIME_VALUE;
    private float totalCookingTime = GlobalConstant.UNUSED_TIME_VALUE;

    private bool isSauceGruyereCooked = false;

    void Awake()
    {
        sauceGruyereVisual = FindFirstObjectByType<SauceGruyereVisual>(FindObjectsInactive.Include);
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }


        if (cookingTime == GlobalConstant.UNUSED_TIME_VALUE) { return; }

        if (cookingTime >= totalCookingTime)
        {
            if (!isSauceGruyereCooked)
            {
                isSauceGruyereCooked = true;
                sauceGruyereVisual.OnSauceGruyereCooked();
            }
            return;
        }

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
        if (!GameManager.Instance.InventoryManager.IsIngredientAvailable(ingredient))
        {
            return;
        }
        if (sauceGruyereIngredients.Contains(ingredient))
        {
            return;
        }
        GameManager.Instance.InventoryManager.ConsumeIngredient(ingredient);
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
        totalCookingTime = sauceGruyere.processingTime;
    }



}