using System.Collections.Generic;
using UnityEngine;

public class TacosMakerManager : MonoBehaviour
{
    [SerializeField] private List<Ingredient> availableIngredients;
    private TacosMakerVisual tacosMakerVisual;

    private Tacos onCreationTacos;
    private List<Tacos> doneTacos = new();
    public readonly int MAX_DONE_TACOS = 2;


    void Awake()
    {
        tacosMakerVisual = FindFirstObjectByType<TacosMakerVisual>(FindObjectsInactive.Include);
    }

    void Start()
    {
        tacosMakerVisual.SetupIngredients(availableIngredients);
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

    public void AddIngredients(Ingredient ingredient)
    {
        onCreationTacos.AddIngredient(ingredient);
    }

    public void WrapTacos()
    {
        if (doneTacos.Count >= MAX_DONE_TACOS)
        {
            return;
        }

        if (onCreationTacos.ingredients.Count == 0)
        {
            return;
        }

        doneTacos.Add(onCreationTacos);
        tacosMakerVisual.WrapTacos(onCreationTacos);
        onCreationTacos = null;
        CreateTacos();
    }
}
