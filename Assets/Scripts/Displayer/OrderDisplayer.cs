using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderDisplayer : MonoBehaviour
{
    public Order orderData;
    [SerializeField] private GameObject ingredientPrefab;

    void Start()
    {
        UpdateOrder();
    }

    void UpdateOrder()
    {
        var index = 0;
        foreach (var ingredient in orderData.expectedOrder)
        {
            var ingredientGO = Instantiate(ingredientPrefab, transform);
            var rectTransform = ingredientGO.GetComponent<RectTransform>();
            ingredientGO.GetComponentInChildren<Image>().sprite = ingredient.processedSprite;

            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(30 + (index % 4) * 60, -30 + (index / 4) * -60);

            index++;
        }
    }
}