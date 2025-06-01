using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IngredientButtonDisplayer : MonoBehaviour
{
    public Ingredient ingredientData;

    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;


    void Start()
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        buttonText.text = ingredientData.name + " " + GameManager.Instance.InventoryManager.GetStockString(ingredientData);
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}