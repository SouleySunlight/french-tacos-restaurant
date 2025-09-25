using System.Collections.Generic;
using UnityEngine;

public class TacosMakerManager : MonoBehaviour
{
    private TacosMakerVisual tacosMakerVisual;

    private Tacos onCreationTacos;


    void Awake()
    {
        tacosMakerVisual = FindFirstObjectByType<TacosMakerVisual>(FindObjectsInactive.Include);
    }

    public void SetupIngredients()
    {
        tacosMakerVisual.SetupIngredients(GetIngredientsAddableToTacos());
        CreateTacos();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        tacosMakerVisual.AddIngredient(ingredient);
    }

    void CreateTacos()
    {
        if (onCreationTacos != null)
        {
            return;
        }

        onCreationTacos = new Tacos();
        tacosMakerVisual.CreateTacos();
    }

    public void AddIngredientsToTacos(Ingredient ingredient)
    {
        if (GameManager.Instance.InventoryManager.IsIngredientAvailableForTacos(ingredient))
        {
            onCreationTacos.AddIngredient(ingredient);
            GameManager.Instance.InventoryManager.ConsumeIngredientForTacos(ingredient);
            tacosMakerVisual.AddIngredientToTacos(ingredient);
        }
    }

    public void AddAvailableIngredient(Ingredient ingredient)
    {
        tacosMakerVisual.AddIngredient(ingredient);
    }

    public Tacos WrapTacos()
    {
        if (onCreationTacos.ingredients.Count == 0)
        {
            throw new EmptyTacosException();
        }

        var wrappedTacos = onCreationTacos;
        tacosMakerVisual.WrapTacos(wrappedTacos);
        onCreationTacos = null;
        CreateTacos();
        return wrappedTacos;
    }

    public void DiscardTacos()
    {
        tacosMakerVisual.DiscardTacos();
        onCreationTacos = null;
        CreateTacos();
    }

    public void UpdateButtonsVisualQuantity()
    {
        tacosMakerVisual.UpdateButtonsVisualQuantity();
    }

    public int GetNumberOfSauceOfOnCreationTacos()
    {
        return onCreationTacos.GetNumberOfSauceInside();
    }

    List<Ingredient> GetIngredientsAddableToTacos()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll(ingredient => ingredient.canBeAddedToTacos);
    }

    public RectTransform GetIngredientButtonTransform(Ingredient ingredient)
    {
        return tacosMakerVisual.GetIngredientButtonTransform(ingredient);
    }

    public RectTransform GetOnCreationTacosTransform()
    {
        return tacosMakerVisual.GetOnCreationTacosTransform();
    }

    public bool IsIngredientInTacos(Ingredient ingredient)
    {
        return onCreationTacos.ingredients.Contains(ingredient);
    }

    public bool IsOnCreationTacosEmpty()
    {
        return onCreationTacos.ingredients.Count == 0;
    }
}
