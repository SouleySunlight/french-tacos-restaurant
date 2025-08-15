using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderDisplayer : MonoBehaviour
{
    public Order orderData;
    [SerializeField] private GameObject ingredientPrefab;
    private List<GameObject> ingredientObjects = new();

    void Start()
    {
        UpdateOrder();
    }

    void UpdateOrder()
    {
        InstantiateMeat();
        InstantiateSauce();
        InstantiateVegetable();
        InstantiateFries();
        InstantiateSauceGruyere();

        UpdatePosition();
    }

    void InstantiateMeat()
    {
        var meats = orderData.expectedOrder.FindAll(x => x.category == IngredientCategoryEnum.MEAT);
        foreach (var meat in meats)
        {
            var ingredientGO = Instantiate(ingredientPrefab, transform);
            ingredientGO.GetComponentInChildren<Image>().sprite = meat.processedSprite;

            ingredientObjects.Add(ingredientGO);
        }
    }

    void InstantiateSauce()
    {
        var sauces = orderData.expectedOrder.FindAll(x => x.category == IngredientCategoryEnum.SAUCE);
        foreach (var sauce in sauces)
        {
            var ingredientGO = Instantiate(ingredientPrefab, transform);
            ingredientGO.GetComponentInChildren<Image>().sprite = sauce.processedSprite;

            ingredientObjects.Add(ingredientGO);
        }
    }

    void InstantiateVegetable()
    {
        var vegetables = orderData.expectedOrder.FindAll(x => x.category == IngredientCategoryEnum.VEGETABLE);
        foreach (var vegetable in vegetables)
        {
            var ingredientGO = Instantiate(ingredientPrefab, transform);
            ingredientGO.GetComponentInChildren<Image>().sprite = vegetable.processedSprite;

            ingredientObjects.Add(ingredientGO);
        }
    }

    void InstantiateFries()
    {
        var fries = orderData.expectedOrder.FindAll(x => x.category == IngredientCategoryEnum.FRIES);
        foreach (var fry in fries)
        {
            var ingredientGO = Instantiate(ingredientPrefab, transform);
            ingredientGO.GetComponentInChildren<Image>().sprite = fry.processedSprite;

            ingredientObjects.Add(ingredientGO);
        }
    }

    void InstantiateSauceGruyere()
    {
        var sauceGruyeres = orderData.expectedOrder.FindAll(x => x.category == IngredientCategoryEnum.SAUCE_GRUYERE);
        foreach (var sauceGruyere in sauceGruyeres)
        {
            var ingredientGO = Instantiate(ingredientPrefab, transform);
            ingredientGO.GetComponentInChildren<Image>().sprite = sauceGruyere.processedSprite;

            ingredientObjects.Add(ingredientGO);
        }
    }

    void UpdatePosition()
    {
        var index = 0;
        foreach (var ingredient in ingredientObjects)
        {
            var rectTransform = ingredient.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(30 + (index % 4) * 60, -30 + (index / 4) * -60);

            index++;
        }
    }
}