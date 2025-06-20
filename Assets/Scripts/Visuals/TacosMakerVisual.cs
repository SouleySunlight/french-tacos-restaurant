using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TacosMakerVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject tortillaPrefab;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private RectTransform onCreationTacosTransform;
    [SerializeField] private RectTransform meatIngredientFirstButtonTransform;
    [SerializeField] private RectTransform sauceIngredientFirstButtonTransform;
    [SerializeField] private RectTransform vegetableIngredientFirstButtonTransform;
    [SerializeField] private RectTransform inEveryTacosIngredientFirstButtonTransform;

    [SerializeField] private RectTransform doneTacosFirstPosition;

    private List<GameObject> buttons = new();

    private GameObject onCreationTacos;
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 2;
    private readonly int NUMBER_OF_VEGETABLE_BUTTON_PER_ROW = 1;


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
        onCreationTacos = Instantiate(tortillaPrefab, onCreationTacosTransform.position, Quaternion.identity, onCreationTacosTransform);
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            AddIngredient(ingredient);
        }
        PlaceButtons();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, meatIngredientFirstButtonTransform.position, Quaternion.identity, meatIngredientFirstButtonTransform);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;

        buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => OnClickToAddIngredient(ingredient));
        buttons.Add(buttonPrefab);
        PlaceButtons();
    }

    void PlaceButtons()
    {
        PlaceButtonsByCategory(IngredientCategoryEnum.MEAT, meatIngredientFirstButtonTransform);
        PlaceButtonsByCategory(IngredientCategoryEnum.SAUCE, sauceIngredientFirstButtonTransform);
        PlaceButtonsByCategory(IngredientCategoryEnum.VEGETABLE, vegetableIngredientFirstButtonTransform, true);
        PlaceInEveryTacosButtons();

    }

    void PlaceButtonsByCategory(IngredientCategoryEnum category, RectTransform firstButtonTransform, bool areVegetables = false)
    {
        var index = 0;
        var categoryButtons = buttons.FindAll((button) => button.GetComponent<IngredientButtonDisplayer>().ingredientData.category == category);
        for (int i = 0; i < categoryButtons.Count; i++)
        {
            var buttonPosition = new Vector3(
                            firstButtonTransform.position.x + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (areVegetables ? index % NUMBER_OF_VEGETABLE_BUTTON_PER_ROW : index % NUMBER_OF_BUTTON_PER_ROW),
                            firstButtonTransform.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (areVegetables ? index / NUMBER_OF_VEGETABLE_BUTTON_PER_ROW : index / NUMBER_OF_BUTTON_PER_ROW),
                            firstButtonTransform.position.z
                        );

            categoryButtons[i].GetComponent<RectTransform>().position = buttonPosition;
            index++;
        }
    }

    void PlaceInEveryTacosButtons()
    {
        var index = 0;
        var inEveryTacosButtons = buttons.FindAll((button) => button.GetComponent<IngredientButtonDisplayer>().ingredientData.inEveryTacos);
        for (int i = 0; i < inEveryTacosButtons.Count; i++)
        {
            var buttonPosition = new Vector3(
                            inEveryTacosIngredientFirstButtonTransform.position.x + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                            inEveryTacosIngredientFirstButtonTransform.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
                            inEveryTacosIngredientFirstButtonTransform.position.z
                        );

            inEveryTacosButtons[i].GetComponent<RectTransform>().position = buttonPosition;
            index++;
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
        buttons.Find(button => button.GetComponent<IngredientButtonDisplayer>().ingredientData == ingredient).GetComponent<IngredientButtonDisplayer>().UpdateVisual();
    }

    public void WrapTacos(Tacos createdTacos)
    {
        Destroy(onCreationTacos);
        onCreationTacos = null;
    }
}
