using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IngredientButtonDisplayer : MonoBehaviour
{

    public Ingredient ingredientData;

    [SerializeField] private Button button;
    [SerializeField] private Image ingredientImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private TMP_Text quantityText;

    [SerializeField] private GameObject shadow;
    [SerializeField] private RectTransform buttonTransform;
    private bool shouldShowUnprocessedIngredient = false;


    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        ingredientImage.sprite = shouldShowUnprocessedIngredient ? ingredientData.unprocessedSprite : ingredientData.processedSprite;
        UpdateQuantity();
        UpdateBorderColor();
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public void UpdateQuantity()
    {
        if (ingredientData.NeedProcessing() && !shouldShowUnprocessedIngredient)
        {
            quantityText.text = GameManager.Instance.InventoryManager.GetProcessedIngredientStockString(ingredientData);
            return;
        }
        quantityText.text = "$ " + ingredientData.priceToRefill;
        return;
    }

    void UpdateBorderColor()
    {
        borderImage.color = ingredientData.category switch
        {
            IngredientCategoryEnum.MEAT => Colors.GetColorFromHexa(Colors.BROWN_MEAT),
            IngredientCategoryEnum.VEGETABLE => Colors.GetColorFromHexa(Colors.GREEN_VEGETABLE),
            IngredientCategoryEnum.SAUCE => Colors.GetColorFromHexa(Colors.YELLOW_SAUCE),
            _ => Colors.GetColorFromHexa(Colors.GREY_EVERY_TACOS),
        };
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

    public void SetShouldShowUnprocessedIngredient(bool shouldShow)
    {
        shouldShowUnprocessedIngredient = shouldShow;
        UpdateVisual();
    }
}