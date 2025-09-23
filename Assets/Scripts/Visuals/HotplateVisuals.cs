using System.Collections.Generic;
using UnityEngine;

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
        UpdateIndicatorsVisual();
        UpdateIndicatorsQuantity();
        GameManager.Instance.HotplateManager.ManageCookingSoundOnViewChanged();
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
        var totalWidth = GetComponent<RectTransform>().rect.width;

        UIPlacement.PlaceIngredientButtons(buttons, totalWidth);
    }

    void UpdateIndicatorsVisual()
    {
        var totalWidth = GetComponent<RectTransform>().rect.width;

        UIPlacement.PlaceIngredientIndicators(indicators, totalWidth);
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

    public void UpdateIndicatorsQuantity()
    {
        foreach (var indicator in indicators)
        {
            indicator.GetComponent<IngredientIndicatorDisplayer>().UpdateVisual();
        }
    }

    void AddTimers()
    {
        for (int i = 0; i < GlobalConstant.MAX_COOKING_INGREDIENTS; i++)
        {
            var completionBar = Instantiate(roundedCompletionBarPrefab, hotplateTransform);
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

    public RectTransform GetIngredientButtonTransform(Ingredient ingredient)
    {
        var button = buttons.Find(button => button.GetComponent<IngredientButtonDisplayer>().ingredientData == ingredient);
        if (button == null)
        {
            return null;
        }
        return button.GetComponent<RectTransform>();
    }

    public RectTransform GetCookingIngredientTransform(int index)
    {
        if (index < 0 || index >= ingredients.Count)
        {
            return null;
        }
        var ingredient = ingredients[index];
        if (ingredient == null)
        {
            return null;
        }
        return ingredient.GetComponent<RectTransform>();
    }
}
