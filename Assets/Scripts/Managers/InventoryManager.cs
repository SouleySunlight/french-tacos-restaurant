using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private List<Ingredient> initialAvailableIngredients = new();
    [HideInInspector] public List<Ingredient> UnlockedIngredients { get; private set; }

    private void Awake()
    {
        UnlockedIngredients = initialAvailableIngredients;
    }
}