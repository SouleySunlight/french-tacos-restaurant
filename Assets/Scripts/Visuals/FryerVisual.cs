using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryerVisual : MonoBehaviour, IView
{

    [SerializeField] private RectTransform firstButtonPosition;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private List<GameObject> quantityManager = new();
    [SerializeField] private List<RectTransform> basketsPosition = new();
    [SerializeField] private List<Image> cookingTimers = new();

    private List<GameObject> ingredients = new();
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;

    private List<GameObject> buttons = new();

    public void Setup()
    {
        for (int i = 0; i < GlobalConstant.MAX_FRYING_INGREDIENTS; i++)
        {
            ingredients.Add(null);
        }
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            AddAvailableIngredient(ingredient);
        }
        UpdateVisual();
    }
    public void AddAvailableIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, firstButtonPosition.position, Quaternion.identity, firstButtonPosition);
        buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().ingredientData = ingredient;
        buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().shouldShowUnprocessedQuantity = true;


        buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().AddListener(() => GameManager.Instance.FryerManager.FryIngredients(ingredient));
        buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().UpdateVisual();
        buttons.Add(buttonPrefab);
        UpdateVisual();
    }

    void UpdateVisual()
    {
        var index = 0;
        foreach (var button in buttons)
        {
            var buttonPosition = new Vector3(
                firstButtonPosition.position.x + GlobalConstant.LEGACY_INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                firstButtonPosition.position.y + GlobalConstant.LEGACY_INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
                firstButtonPosition.position.z
            );

            button.GetComponent<RectTransform>().position = buttonPosition;
            index++;
        }
    }

    public void FryIngredients(Ingredient ingredient, int position)
    {
        var ingredientToFry = Instantiate(ingredientPrefab, basketsPosition[position]);
        ingredientToFry.GetComponent<IngredientMovement>().ClickFryerEvent.AddListener(OnClickOnIngredient);
        ingredientToFry.GetComponent<IngredientDisplayer>().ingredientData = ingredient;

        if (ingredients[position] == null)
        {
            ingredients[position] = ingredientToFry;
            quantityManager[position].GetComponent<QuantityDisplayer>().SetQuantity(1);
        }
        else
        {
            quantityManager[position].GetComponent<QuantityDisplayer>().SetQuantity(quantityManager[position].GetComponent<QuantityDisplayer>().currentQuantity + 1);

        }
        PlaceIngredients(ingredientToFry, position, quantityManager[position].GetComponent<QuantityDisplayer>().currentQuantity);

        UpdateIngredientButtons();
    }

    public void UpdateIngredientButtons()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<LegacyIngredientButtonDisplayer>().UpdateVisual();
        }
    }

    void OnClickOnIngredient(GameObject gameObject)
    {
        GameManager.Instance.FryerManager.OnIngredientClick(ingredients.FindIndex(ingredient => ingredient == gameObject));
    }

    public void UpdateTimer(int index, float percentage)
    {
        cookingTimers[index].fillAmount = percentage;
    }

    public void OnIngredientCooked(int position)
    {
        ingredients[position].GetComponent<IngredientDisplayer>().DisplayProcessedImage();
    }

    public void OnIngredientBurnt(int position)
    {
        ingredients[position].GetComponent<IngredientDisplayer>().DisplayWastedImage();
    }


    public void RemoveIngredientFromGrill(int position)
    {
        var ingredientToRemove = ingredients[position];
        Destroy(ingredientToRemove);
        ingredients[position] = null;
        UpdateTimer(position, 0);
        quantityManager[position].GetComponent<QuantityDisplayer>().SetQuantity(0);
        UpdateIngredientButtons();
    }

    public void OnViewDisplayed()
    {
        UpdateIngredientButtons();
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