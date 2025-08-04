using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryerVisual : MonoBehaviour, IView
{

    [SerializeField] private RectTransform firstButtonPosition;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private List<GameObject> quantityManager = new();
    [SerializeField] private List<GameObject> baskets = new();
    [SerializeField] private List<Image> cookingTimers = new();
    [SerializeField] private GameObject ingredientIndicatorPrefab;
    private List<List<GameObject>> ingredientsInBasket = new();
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;

    private List<GameObject> buttons = new();
    private List<GameObject> indicators = new();

    public void Setup()
    {
        for (int i = 0; i < GlobalConstant.MAX_FRYING_INGREDIENTS; i++)
        {
            ingredientsInBasket.Add(new List<GameObject>());
        }
        foreach (var basket in baskets)
        {
            basket.GetComponent<BasketMovement>().ClickBasket.AddListener(OnClickOnBasket);
        }
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            AddAvailableIngredient(ingredient);
            AddIngredientIndicator(ingredient);
        }
        UpdateVisual();
    }
    public void AddAvailableIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, this.transform);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().SetShouldShowUnprocessedIngredient(true);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => GameManager.Instance.FryerManager.FryIngredients(ingredient));
        buttons.Add(buttonPrefab);
        UpdateButtonsVisual();
    }

    public void AddIngredientIndicator(Ingredient ingredient)
    {
        var indicator = Instantiate(ingredientIndicatorPrefab, this.transform);
        indicator.GetComponent<IngredientIndicatorDisplayer>().ingredientData = ingredient;
        indicator.GetComponent<IngredientIndicatorDisplayer>().UpdateVisual();

        indicators.Add(indicator);
        UpdateIndicatorsVisual();
    }

    void UpdateVisual()
    {
        UpdateButtonsVisual();
        UpdateIndicatorsVisual();
        UpdateIndicatorsQuantity();
    }

    void UpdateButtonsVisual()
    {
        var totalWidth = GetComponent<RectTransform>().rect.width;
        UIPlacement.PlaceIngredientButtons(buttons, totalWidth);
    }

    void UpdateIndicatorsVisual()
    {
        var totalWidth = GetComponent<RectTransform>().rect.width;
        UIPlacement.PlaceIngredientIndicators(indicators, totalWidth);
    }

    public void UpdateIndicatorsQuantity()
    {
        foreach (var indicator in indicators)
        {
            indicator.GetComponent<IngredientIndicatorDisplayer>().UpdateVisual();
        }
    }

    public void FryIngredients(Ingredient ingredient, int position)
    {
        var ingredientToFry = Instantiate(ingredientPrefab, baskets[position].GetComponent<RectTransform>());
        ingredientToFry.GetComponent<IngredientDisplayer>().ingredientData = ingredient;
        ingredientsInBasket[position].Add(ingredientToFry);

        quantityManager[position].GetComponent<QuantityDisplayer>().SetQuantity(quantityManager[position].GetComponent<QuantityDisplayer>().currentQuantity + 1);

        PlaceIngredients(ingredientToFry, position, quantityManager[position].GetComponent<QuantityDisplayer>().currentQuantity);

        UpdateIngredientButtons();
    }

    public void UpdateIngredientButtons()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<IngredientButtonDisplayer>().UpdateVisual();
        }
    }

    void OnClickOnBasket(GameObject gameObject)
    {
        var index = baskets.FindIndex(basket => basket == gameObject);
        baskets[index].transform.SetAsFirstSibling();
        GameManager.Instance.FryerManager.OnClickOnBasket(index);
    }

    public void UpdateTimer(int index, float percentage)
    {
        cookingTimers[index].fillAmount = percentage;
    }

    public void OnIngredientCooked(int position)
    {
        foreach (var ingredient in ingredientsInBasket[position])
        {
            ingredient.GetComponent<IngredientDisplayer>().DisplayProcessedInFryerImage();
        }
    }

    public void OnIngredientBurnt(int position)
    {
        foreach (var ingredient in ingredientsInBasket[position])
        {
            ingredient.GetComponent<IngredientDisplayer>().DisplayWastedInFryerImage();
        }

    }


    public void RemoveIngredientFromFryer(int position)
    {
        baskets[position].transform.SetAsLastSibling();
        foreach (var ingredient in ingredientsInBasket[position])
        {
            Destroy(ingredient);
        }
        ingredientsInBasket[position].Clear();
        UpdateTimer(position, 0);
        quantityManager[position].GetComponent<QuantityDisplayer>().SetQuantity(0);
        UpdateIngredientButtons();
        UpdateIndicatorsQuantity();
    }

    public void OnViewDisplayed()
    {
        UpdateVisual();
    }

    public void PlaceIngredients(GameObject ingredient, int position, int quantity)
    {
        var rect = ingredient.GetComponent<RectTransform>();

        if (quantity == 1)
        {
            rect.anchoredPosition = new(0, 100);
            rect.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        if (quantity == 2)
        {
            rect.anchoredPosition = new(0, 0);
            rect.rotation = Quaternion.Euler(0, 0, 30);
            return;
        }
        if (quantity == 3)
        {
            rect.anchoredPosition = new(0, 200);
            rect.rotation = Quaternion.Euler(0, 0, -30);
            return;
        }
    }

}