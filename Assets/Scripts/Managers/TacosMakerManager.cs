using System.Collections.Generic;
using UnityEngine;

public class TacosMakerManager : MonoBehaviour
{
    [SerializeField] private List<Ingredient> availableIngredients;
    private TacosMakerVisual tacosMakerVisual;

    private Tacos onCreationTacos;


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
}
