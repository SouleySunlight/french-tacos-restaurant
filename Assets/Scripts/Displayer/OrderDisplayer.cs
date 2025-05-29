using TMPro;
using UnityEngine;

public class OrderDisplayer : MonoBehaviour
{
    public Order orderData;
    public TMP_Text orderText;

    void Start()
    {
        var text = "";

        foreach (var row in orderData.expectedOrder)
        {
            text += "- ";
            foreach (var ingredient in row.tacosIngredients)
            {
                text += ingredient.name + " ";
            }
            text += "<br>";
        }

        orderText.text = text;
    }
}