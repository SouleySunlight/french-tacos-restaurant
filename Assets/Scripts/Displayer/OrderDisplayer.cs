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
            foreach (var ingredient in orderItem.tacosIngredients)
            {
                text += ingredient.name + " ";
            }
            if (orderItem.isServed) { text += "</s>"; }
            text += "<br>";
        }

        orderText.text = text;
    }
}