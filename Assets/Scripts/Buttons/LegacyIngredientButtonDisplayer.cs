using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LegacyIngredientButtonDisplayer : MonoBehaviour
{
    public Ingredient ingredientData;
    public bool shouldShowQuantity = false;
    public bool shouldShowUnprocessedQuantity = false;
    public bool shouldShowUnlockPrice = false;
    public bool shouldShowRefillPrice = false;

    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;


    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        buttonText.text = ingredientData.name;
        if (shouldShowQuantity)
        {
            buttonText.text += " " + GameManager.Instance.InventoryManager.GetProcessedIngredientStockString(ingredientData);
        }
        if (shouldShowUnprocessedQuantity)
        {
            buttonText.text += " TEMP";

        }
        if (shouldShowUnlockPrice)
        {
            buttonText.text += " " + ingredientData.priceToUnlock + " €";
        }
        if (shouldShowRefillPrice)
        {
            buttonText.text += " " + ingredientData.priceToRefill + " €";
        }
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}