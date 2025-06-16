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
    private bool isSauceGruyereBurnt = false;
    private static readonly int SAUCE_GRUYERE_CREATED_QUANTITY = 5;


    void Awake()
    {
        sauceGruyereVisual = FindFirstObjectByType<SauceGruyereVisual>(FindObjectsInactive.Include);
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }


        if (cookingTime == GlobalConstant.UNUSED_TIME_VALUE) { return; }

        if (cookingTime >= totalCookingTime + sauceGruyere.wastingTimeOffset)
        {
            if (!isSauceGruyereBurnt)
            {
                isSauceGruyereBurnt = true;
                sauceGruyereVisual.OnSauceGruyereBurnt();
            }
        }

        if (cookingTime >= totalCookingTime && cookingTime < totalCookingTime + sauceGruyere.wastingTimeOffset)
        {
            if (!isSauceGruyereCooked)
            {
                isSauceGruyereCooked = true;
                sauceGruyereVisual.OnSauceGruyereCooked();
            }
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

    public void OnClickOnIngredient(Ingredient ingredient)
    {
        if (ingredient.id != sauceGruyere.id)
        {
            return;
        }
        if (isSauceGruyereBurnt)
        {
            RemoveSauceGruyere();
            return;
        }
        if (isSauceGruyereCooked)
        {
            GameManager.Instance.InventoryManager.AddIngredient(sauceGruyere, SAUCE_GRUYERE_CREATED_QUANTITY);
            RemoveSauceGruyere();
            return;
        }
    }

    public void RemoveSauceGruyere()
    {
        sauceGruyereIngredients.Clear();
        sauceGruyereVisual.RemoveSauceGruyere();
        cookingTime = GlobalConstant.UNUSED_TIME_VALUE;
        totalCookingTime = GlobalConstant.UNUSED_TIME_VALUE;
        isSauceGruyereCooked = false;
        isSauceGruyereBurnt = false;
        sauceGruyereVisual.UpdateTimer(0);

    }



}