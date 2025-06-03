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
        tacosMakerVisual.SetupIngredients(GameManager.Instance.InventoryManager.UnlockedIngredients);
        CreateTacos();
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
        if (GameManager.Instance.InventoryManager.IsIngredientAvailable(ingredient))
        {
            onCreationTacos.AddIngredient(ingredient);
            GameManager.Instance.InventoryManager.ConsumeIngredient(ingredient);
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
}
