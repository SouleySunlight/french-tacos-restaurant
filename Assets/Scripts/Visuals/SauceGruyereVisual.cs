
using System.Collections.Generic;
using UnityEngine;

public class SauceGruyereVisual : MonoBehaviour, IView
{
    [SerializeField] private RectTransform firstIngredientPosition;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private RectTransform sauceGruyerePosition;

    private List<GameObject> buttons = new();
    private List<GameObject> ingredientsInSauceGruyere = new();
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;


    public void AddAvailableIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, firstIngredientPosition.position, Quaternion.identity, firstIngredientPosition);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().shouldShowUnprocessedQuantity = true;


        buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => GameManager.Instance.SauceGruyereManager.AddIngredientToSauceGruyere(ingredient));
        buttons.Add(buttonPrefab);
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

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            AddAvailableIngredient(ingredient);
        }
        UpdateVisual();
    }

    public void AddIngredientToSauceGruyere(Ingredient ingredient)
    {
        var ingredientToCook = Instantiate(ingredientPrefab, sauceGruyerePosition.position, Quaternion.identity, sauceGruyerePosition);
        ingredientToCook.GetComponent<IngredientDisplayer>().ingredientData = ingredient;
        ingredientsInSauceGruyere.Add(ingredientToCook);
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