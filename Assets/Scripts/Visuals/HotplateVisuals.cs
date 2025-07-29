using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotplateVisuals : MonoBehaviour, IView
{
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private GameObject roundedCompletionBarPrefab;
    [SerializeField] private GameObject ingredientIndicatorPrefab;
    private List<GameObject> ingredients = new();
    [SerializeField] private RectTransform hotplateTransform;

    private List<GameObject> buttons = new();
    private List<GameObject> indicators = new();

    private List<GameObject> completionBars = new();

    private readonly int NUMBER_OF_BUTTON_PER_ROW = 4;
    private readonly int NUMBER_OF_INDICATOR_PER_ROW = 3;


    public void Setup()
    {
        for (int i = 0; i < GlobalConstant.MAX_COOKING_INGREDIENTS; i++)
        {
            ingredients.Add(null);
        }
    }

    public void OnViewDisplayed()
    {
        UpdateIngredientButtons();
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            AddAvailableIngredient(ingredient);
            AddIngredientIndicator(ingredient);
        }
        AddTimers();
    }

    void UpdateButtonsVisual()
    {
        const float buttonWidth = 120f;
        const float buttonHeight = 120f;

        var totalButtons = buttons.Count;
        var totalWidth = GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < totalButtons; i++)
        {
            var button = buttons[i].GetComponent<RectTransform>();

            button.anchorMin = new Vector2(0, 0.6f);
            button.anchorMax = new Vector2(0, 0.6f);
            button.pivot = new Vector2(0.5f, 0f);

            int col = i % NUMBER_OF_BUTTON_PER_ROW;
            int row = i / NUMBER_OF_BUTTON_PER_ROW;

            int itemsInRow = Mathf.Min(totalButtons - row * NUMBER_OF_BUTTON_PER_ROW, NUMBER_OF_BUTTON_PER_ROW);

            float totalRowWidth = itemsInRow * buttonWidth;

            float spaceLeft = totalWidth - totalRowWidth;

            float gap = spaceLeft / (itemsInRow + 1);

            float x = gap * (col + 1) + buttonWidth * col + 100;

            float y = -(buttonHeight + 20f) * row;

            button.anchoredPosition = new Vector2(x, y);
        }
    }

    void UpdateIndicatorsVisual()
    {
        const float indicatorWidth = 230f;
        const float indicatorHeight = 110f;

        var totalIndicators = indicators.Count;
        var totalWidth = GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < totalIndicators; i++)
        {
            var indicator = indicators[i].GetComponent<RectTransform>();

            indicator.anchorMin = new Vector2(0, 0.8f);
            indicator.anchorMax = new Vector2(0, 0.8f);
            indicator.pivot = new Vector2(0.5f, 0f);

            int col = i % NUMBER_OF_INDICATOR_PER_ROW;
            int row = i / NUMBER_OF_INDICATOR_PER_ROW;

            int itemsInRow = Mathf.Min(totalIndicators - row * NUMBER_OF_INDICATOR_PER_ROW, NUMBER_OF_INDICATOR_PER_ROW);

            float totalRowWidth = itemsInRow * indicatorWidth;

            float spaceLeft = totalWidth - totalRowWidth;

            float gap = spaceLeft / (itemsInRow + 1);

            float x = gap * (col + 1) + indicatorWidth * col + 175;

            float y = -(indicatorHeight + 20f) * row - 30;

            indicator.anchoredPosition = new Vector2(x, y);
        }
    }

    public void AddAvailableIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, this.transform);

        buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().SetShouldShowUnprocessedIngredient(true);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => GameManager.Instance.HotplateManager.CookIngredients(ingredient));

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

    void AddTimers()
    {
        for (int i = 0; i < GlobalConstant.MAX_COOKING_INGREDIENTS; i++)
        {
            var completionBar = Instantiate(roundedCompletionBarPrefab, hotplateTransform);
            completionBar.GetComponent<RoundedCompletionBarDisplayer>().UpdateTimer(0);
            completionBars.Add(completionBar);

            var rectTransform = completionBar.GetComponent<RectTransform>();

            var positionX = i % 2 == 0 ? 0.25f : 0.75f;
            var positionY = i > 1 ? 0.33f : 0.6f;

            rectTransform.anchorMin = new Vector2(positionX, positionY);
            rectTransform.anchorMax = new Vector2(positionX, positionY);
            rectTransform.pivot = new Vector2(0.5f, 0f);

            rectTransform.anchoredPosition = new Vector2(100, 100);
        }

    }

    public void CookIngredients(Ingredient ingredient, int position)
    {
        var ingredientToCook = Instantiate(ingredientPrefab, hotplateTransform);
        ingredientToCook.transform.SetAsFirstSibling();

        var rectTransform = ingredientToCook.GetComponent<RectTransform>();

        var positionX = position % 2 == 0 ? 0.25f : 0.75f;
        var positionY = position > 1 ? 0.33f : 0.6f;

        rectTransform.anchorMin = new Vector2(positionX, positionY);
        rectTransform.anchorMax = new Vector2(positionX, positionY);
        rectTransform.pivot = new Vector2(0.5f, 0f);


        ingredientToCook.GetComponent<IngredientDisplayer>().ingredientData = ingredient;

        ingredientToCook.GetComponent<IngredientMovement>().ClickHotplateEvent.AddListener(OnClickOnIngredient);
        ingredients[position] = ingredientToCook;
        UpdateIngredientButtons();
    }

    public void OnIngredientCooked(int position)
    {
        ingredients[position].GetComponent<IngredientDisplayer>().DisplayProcessedImage();
    }

    public void OnIngredientBurnt(int position)
    {
        ingredients[position].GetComponent<IngredientDisplayer>().DisplayWastedImage();
    }

    public void UpdateTimer(int index, float percentage)
    {
        completionBars[index].GetComponent<RoundedCompletionBarDisplayer>().UpdateTimer(percentage);
    }

    void OnClickOnIngredient(GameObject gameObject)
    {
        GameManager.Instance.HotplateManager.OnClickOnIngredient(ingredients.FindIndex(ingredient => ingredient == gameObject));
    }

    public void RemoveIngredientFromGrill(int position)
    {
        var ingredientToRemove = ingredients[position];
        Destroy(ingredientToRemove);
        ingredients[position] = null;
        UpdateTimer(position, 0);
        UpdateIngredientButtons();
    }

    public void UpdateIngredientButtons()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<IngredientButtonDisplayer>().GetComponent<IngredientButtonDisplayer>().UpdateVisual();
        }
    }
}
