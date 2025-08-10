using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientIndicatorDisplayer : MonoBehaviour
{
    public Ingredient ingredientData;

    [SerializeField] private Image ingredientImage;
    [SerializeField] private TMP_Text ingredientQuantityText;

    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        ingredientImage.sprite = ingredientData.processedSprite;
        ingredientQuantityText.text = GameManager.Instance.InventoryManager.GetProcessedIngredientStockString(ingredientData);
    }
}