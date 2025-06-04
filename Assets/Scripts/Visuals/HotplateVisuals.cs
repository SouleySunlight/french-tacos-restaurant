using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotplateVisuals : MonoBehaviour, IView
{
    [SerializeField] private RectTransform firstIngredientPosition;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private List<GameObject> ingredients;
    [SerializeField] private List<RectTransform> cookPositions = new();
    [SerializeField] private List<Image> cookingTimers = new();

    private List<GameObject> buttons = new();

    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;

    void Start()
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
        }
        UpdateVisual();
    }

    void UpdateVisual()
    {
        var index = 0;
        foreach (var button in buttons)
        {
            var buttonPosition = new Vector3(
                firstIngredientPosition.position.x + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                firstIngredientPosition.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
                firstIngredientPosition.position.z
            );

            button.GetComponent<RectTransform>().position = buttonPosition;
            index++;
        }
    }

    public void AddAvailableIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, firstIngredientPosition.position, Quaternion.identity, firstIngredientPosition);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().shouldShowUnprocessedQuantity = true;


        buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => GameManager.Instance.HotplateManager.CookIngredients(ingredient));
        buttons.Add(buttonPrefab);
        UpdateVisual();
    }

    public void CookIngredients(Ingredient ingredient, int position)
    {
        var ingredientToCook = Instantiate(ingredientPrefab, cookPositions[position].position, Quaternion.identity, cookPositions[position]);
        ingredientToCook.GetComponent<IngredientDisplayer>().ingredientData = ingredient;
        ingredientToCook.GetComponent<IngredientMovement>().ClickHotplateEvent.AddListener(OnClickOnIngredient);
        ingredients[position] = ingredientToCook;
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
        cookingTimers[index].fillAmount = percentage;
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
