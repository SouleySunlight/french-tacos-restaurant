using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TacosMakerVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject tortillaPrefab;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private GameObject trashButtonPrefab;
    [SerializeField] private RectTransform inEveryTacosIngredientFirstButtonTransform;

    private List<GameObject> buttons = new();

    private GameObject onCreationTacos;
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 2;


    public void OnViewDisplayed()
    {
        UpdateButtonsVisualQuantity();
    }

    public void UpdateButtonsVisualQuantity()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<IngredientButtonDisplayer>().GetComponent<IngredientButtonDisplayer>().UpdateQuantity();
        }
    }

    public void CreateTacos()
    {
        onCreationTacos = Instantiate(tortillaPrefab, this.transform);

        var rectTransform = onCreationTacos.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0.5f, 0.15f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.15f);
        rectTransform.pivot = new Vector2(0.5f, 0f);

        rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            AddIngredient(ingredient);
        }
        PlaceButtons();
        CreateTrashButton();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, this.transform);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;

        buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => OnClickToAddIngredient(ingredient));
        buttons.Add(buttonPrefab);
        PlaceButtons();
    }

    void PlaceButtons()
    {
        PlaceMeatButtons();
        PlaceSauceButtons();
        PlaceVegetableButtons();
        PlaceInEveryTacosButtons();

    }

    void CreateTrashButton()
    {
        var trashButton = Instantiate(trashButtonPrefab, this.transform);
        trashButton.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.TacosMakerManager.DiscardTacos());
        var rectTransform = trashButton.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0.05f, 0.025f);
        rectTransform.anchorMax = new Vector2(0.05f, 0.025f);
        rectTransform.pivot = new Vector2(0.5f, 0f);

        rectTransform.anchoredPosition = new Vector2(100, 0);
    }

    void PlaceMeatButtons()
    {
        var meatButtons = buttons.FindAll((button) => button.GetComponent<IngredientButtonDisplayer>().ingredientData.category == IngredientCategoryEnum.MEAT);
        for (int i = 0; i < meatButtons.Count; i++)
        {
            var rectTransform = meatButtons[i].GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0f, 0.75f);
            rectTransform.anchorMax = new Vector2(0f, 0.75f);
            rectTransform.pivot = new Vector2(0.5f, 0f);

            rectTransform.anchoredPosition = new Vector2(200 + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (i % NUMBER_OF_BUTTON_PER_ROW), GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (i / NUMBER_OF_BUTTON_PER_ROW));
        }

    }

    void PlaceSauceButtons()
    {
        var sauceButtons = buttons.FindAll((button) => button.GetComponent<IngredientButtonDisplayer>().ingredientData.category == IngredientCategoryEnum.SAUCE);
        for (int i = 0; i < sauceButtons.Count; i++)
        {
            var rectTransform = sauceButtons[i].GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0f, 0.75f);
            rectTransform.anchorMax = new Vector2(0f, 0.75f);
            rectTransform.pivot = new Vector2(0.5f, 0f);

            rectTransform.anchoredPosition = new Vector2(500 + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (i % NUMBER_OF_BUTTON_PER_ROW), GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (i / NUMBER_OF_BUTTON_PER_ROW));
        }

    }


    void PlaceVegetableButtons()
    {
        var vegetableButtons = buttons.FindAll((button) => button.GetComponent<IngredientButtonDisplayer>().ingredientData.category == IngredientCategoryEnum.VEGETABLE);
        for (int i = 0; i < vegetableButtons.Count; i++)
        {
            var rectTransform = vegetableButtons[i].GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0f, 0.75f);
            rectTransform.anchorMax = new Vector2(0f, 0.75f);
            rectTransform.pivot = new Vector2(0.5f, 0f);

            rectTransform.anchoredPosition = new Vector2(800, GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * i);
        }

    }


    void PlaceInEveryTacosButtons()
    {
        var inEveryTacosButtons = buttons.FindAll((button) => button.GetComponent<IngredientButtonDisplayer>().ingredientData.inEveryTacos);
        for (int i = 0; i < inEveryTacosButtons.Count; i++)
        {
            var rectTransform = inEveryTacosButtons[i].GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0.4f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.4f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0f);

            rectTransform.anchoredPosition = new Vector2(0 + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (i % NUMBER_OF_BUTTON_PER_ROW), GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (i / NUMBER_OF_BUTTON_PER_ROW));
        }
    }

    void OnClickToAddIngredient(Ingredient ingredient)
    {
        if (onCreationTacos == null) { return; }

        GameManager.Instance.TacosMakerManager.AddIngredientsToTacos(ingredient);
    }
    public void AddIngredientToTacos(Ingredient ingredient)
    {
        var createdIngredient = Instantiate(ingredientPrefab, onCreationTacos.GetComponent<RectTransform>().position, Quaternion.identity, onCreationTacos.GetComponent<RectTransform>());
        createdIngredient.GetComponent<IngredientDisplayer>().ingredientData = ingredient;
        createdIngredient.GetComponent<IngredientMovement>().ClickTacosMakerEvent.AddListener(OnClickOnIngredient);
        var rectTransform = createdIngredient.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        buttons.Find(button => button.GetComponent<IngredientButtonDisplayer>().ingredientData == ingredient).GetComponent<IngredientButtonDisplayer>().UpdateVisual();
    }

    public void WrapTacos(Tacos createdTacos)
    {
        DiscardTacos();
    }

    public void DiscardTacos()
    {
        Destroy(onCreationTacos);
        onCreationTacos = null;
    }

    void OnClickOnIngredient(GameObject gameObject)
    {
        FindFirstObjectByType<GameManager>().WrapTacos();
    }

}
