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
    [SerializeField] private GameObject quantityDisplayer;
    [SerializeField] private GameObject priceDisplayer;
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
            quantityDisplayer.SetActive(true);
            priceDisplayer.SetActive(false);
            quantityDisplayer.GetComponentInChildren<TMP_Text>().text = GameManager.Instance.InventoryManager.GetProcessedIngredientStockString(ingredientData);
            return;
        }
        quantityDisplayer.SetActive(false);
        priceDisplayer.SetActive(true);
        priceDisplayer.GetComponentInChildren<TMP_Text>().text = GameManager.Instance.DayCycleManager.GetCurrentDay() == 0 ? "0" : ingredientData.priceToRefill.ToString();
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

    public void SetShouldShowUnprocessedIngredient(bool shouldShow)
    {
        shouldShowUnprocessedIngredient = shouldShow;
        UpdateVisual();
    }
}