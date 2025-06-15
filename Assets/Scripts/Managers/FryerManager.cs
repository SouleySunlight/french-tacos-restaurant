using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class FryerManager : MonoBehaviour
{
    private FryerVisual fryerVisuals;
    private List<Ingredient> fryingIngredients = new();
    private List<float> fryingTimes = new();
    private List<float> totalFryingTimes = new();
    private List<int> fryingQuantities = new();
    private List<bool> isFrying = new();

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
            isFrying.Add(false);
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
                fryerVisuals.OnIngredientCooked(i);
            }

            if (fryingTimes[i] >= totalFryingTimes[i] + fryingIngredients[i].wastingTimeOffset)
            {
                fryerVisuals.OnIngredientBurnt(i);
            }

            fryingTimes[i] += Time.deltaTime;
            fryerVisuals.UpdateTimer(i, fryingTimes[i] / totalFryingTimes[i]);
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
            if (isFrying[i])
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
            }
            fryingQuantities[i]++;
            fryerVisuals.FryIngredients(ingredient, i);
            return;
        }
        throw new NotEnoughSpaceException();

    }
    public void OnIngredientClick(int position)
    {
        if (!isFrying[position])
        {
            StartFryingIngredient(position);
            return;
        }

        var fryingTime = fryingTimes[position];
        if (fryingTime > totalFryingTimes[position] && fryingTime < totalFryingTimes[position] + fryingIngredients[position].wastingTimeOffset)
        {
            OnIngredientCookedClicked(position);
            return;
        }
        if (fryingTime >= totalFryingTimes[position] + fryingIngredients[position].wastingTimeOffset)
        {
            OnIngredientBurntClicked(position);
            return;
        }
    }

    void StartFryingIngredient(int position)
    {
        if (position < 0 || position >= fryingIngredients.Count)
        {
            return;
        }

        if (fryingIngredients[position] == null)
        {
            return;
        }

        isFrying[position] = true;
        fryingTimes[position] = 0;
        totalFryingTimes[position] = fryingIngredients[position].processingTime * GameManager.Instance.UpgradeManager.GetEffect("FRYER");
    }

    void RemoveIngredientFromFrying(int position)
    {
        fryingIngredients[position] = null;
        fryingTimes[position] = GlobalConstant.UNUSED_TIME_VALUE;
        totalFryingTimes[position] = GlobalConstant.UNUSED_TIME_VALUE;
        isFrying[position] = false;
        fryingQuantities[position] = 0;
        fryerVisuals.RemoveIngredientFromGrill(position);
    }

    void OnIngredientCookedClicked(int position)
    {
        var ingredientToAdd = fryingIngredients[position];
        for (int i = 0; i < fryingQuantities[position]; i++)
        {
            if (GameManager.Instance.InventoryManager.CanAddIngredient(ingredientToAdd))
            {
                GameManager.Instance.InventoryManager.AddIngredient(ingredientToAdd);
            }
        }
        RemoveIngredientFromFrying(position);
    }

    void OnIngredientBurntClicked(int position)
    {
        RemoveIngredientFromFrying(position);
    }
}