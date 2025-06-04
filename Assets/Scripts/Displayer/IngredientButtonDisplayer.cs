using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IngredientButtonDisplayer : MonoBehaviour
{
    public Ingredient ingredientData;
    public bool shouldShowQuantity = false;
    public bool shouldShowUnprocessedQuantity = false;
    public bool shouldShowPrice = false;
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
            buttonText.text += " " + GameManager.Instance.InventoryManager.GetStockString(ingredientData);
        }
        if (shouldShowUnprocessedQuantity)
        {
            buttonText.text += " " + GameManager.Instance.InventoryManager.GetUnprocessedStockString(ingredientData);

        }
        if (shouldShowPrice)
        {
            buttonText.text += " " + ingredientData.priceToUnlock + " €";
        }
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}