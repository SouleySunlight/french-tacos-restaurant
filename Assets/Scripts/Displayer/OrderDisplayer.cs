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

        foreach (var orderItem in orderData.expectedOrder)
        {
            text += "- ";
            if (orderItem.isServed) { text += "<s>"; }
            text += WriteMeat(orderItem) + " - ";
            text += WriteSauces(orderItem);
            text += WriteSauces(orderItem) == "" ? "" : " - ";
            text += WriteVegetables(orderItem);
            if (orderItem.isServed) { text += "</s>"; }
            text += "<br>";
        }

        orderText.text = text;
    }

    public string WriteMeat(OrderItem orderItem)
    {
        var meat = orderItem.tacosIngredients.Find(ingredient => ingredient.category == IngredientCategoryEnum.MEAT);
        return meat.id;
    }

    public string WriteSauces(OrderItem orderItem)
    {
        var sauces = orderItem.tacosIngredients.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.SAUCE);
        var stringedSauces = "";
        for (int i = 0; i < sauces.Count; i++)
        {
            stringedSauces += sauces[i].id;
            stringedSauces += i == sauces.Count - 1 ? "" : "/";
        }
        return stringedSauces;
    }

    public string WriteVegetables(OrderItem orderItem)
    {
        var stringedVegetables = "";

        var vegetables = orderItem.tacosIngredients.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.VEGETABLE);
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