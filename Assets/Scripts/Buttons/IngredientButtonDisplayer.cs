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

    [SerializeField] private GameObject shadow;
    [SerializeField] private RectTransform buttonTransform;


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

    public void OnPressDown()
    {
        var newPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y - 5f);
        buttonTransform.anchoredPosition = newPosition;
        shadow.SetActive(false);
    }

    public void OnRelease()
    {
        var newPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y + 5f);
        buttonTransform.anchoredPosition = newPosition;
        shadow.SetActive(true);
    }
}