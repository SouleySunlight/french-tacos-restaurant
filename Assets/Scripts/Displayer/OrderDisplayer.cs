using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderDisplayer : MonoBehaviour
{
    public Order orderData;
    public TMP_Text orderText;

    void Start()
    {
        UpdateOrder();
    }

    public void UpdateOrder()
    {
        var text = "";

        text += "- ";
        text += WriteMeat(orderData.expectedOrder) + " - ";
        text += WriteSauces(orderData.expectedOrder);
        text += WriteSauces(orderData.expectedOrder) == "" ? "" : " - ";
        text += WriteVegetables(orderData.expectedOrder);
        text += "<br>";

        orderText.text = text;
    }

    public string WriteMeat(List<Ingredient> orderItem)
    {
        var meat = orderItem.Find(ingredient => ingredient.category == IngredientCategoryEnum.MEAT);
        return meat.id;
    }

    public string WriteSauces(List<Ingredient> orderItem)
    {
        var sauces = orderItem.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.SAUCE);
        var stringedSauces = "";
        for (int i = 0; i < sauces.Count; i++)
        {
            stringedSauces += sauces[i].id;
            stringedSauces += i == sauces.Count - 1 ? "" : "/";
        }
        return stringedSauces;
    }

    public string WriteVegetables(List<Ingredient> orderItem)
    {
        var stringedVegetables = "";

        var vegetables = orderItem.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.VEGETABLE);
        if (vegetables.Count == 0)
        {
            return stringedVegetables;
        }
        if (vegetables.Find((vegetable) => vegetable.id == "SAL") != null)
        {
            stringedVegetables += "S";
        }
        if (vegetables.Find((vegetable) => vegetable.id == "TOM") != null)
        {
            stringedVegetables += "T";
        }
        if (vegetables.Find((vegetable) => vegetable.id == "ONI") != null)
        {
            stringedVegetables += "O";
        }
        return stringedVegetables;

    }
}