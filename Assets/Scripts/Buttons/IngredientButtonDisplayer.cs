using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IngredientButtonDisplayer : MonoBehaviour
{

    public Ingredient ingredientData;

    [SerializeField] private Button button;
    [SerializeField] private Image ingredientImage;
    [SerializeField] private TMP_Text quantityText;


    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        ingredientImage.sprite = ingredientData.processedSprite;
        UpdateQuantity();
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public void UpdateQuantity()
    {
        quantityText.text = GameManager.Instance.InventoryManager.GetStockString(ingredientData);
    }
}