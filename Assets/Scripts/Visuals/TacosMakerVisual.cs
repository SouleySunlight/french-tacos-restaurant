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
    [SerializeField] private RectTransform ingredientButtonFirstTransform;
    [SerializeField] private RectTransform doneTacosFirstPosition;

    private List<GameObject> buttons = new();

    private GameObject onCreationTacos;
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;

    public void OnViewDisplayed()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<IngredientButtonDisplayer>().GetComponent<IngredientButtonDisplayer>().UpdateVisual();
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
        UpdateVisual();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, ingredientButtonFirstTransform.position, Quaternion.identity, ingredientButtonFirstTransform);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().shouldShowQuantity = true;

        buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => OnClickToAddIngredient(ingredient));
        buttons.Add(buttonPrefab);
        UpdateVisual();
    }

    void UpdateVisual()
    {
        var index = 0;
        foreach (GameObject button in buttons)
        {
            var buttonPosition = new Vector3(
                ingredientButtonFirstTransform.position.x + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                ingredientButtonFirstTransform.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
                ingredientButtonFirstTransform.position.z
            );

            button.GetComponent<RectTransform>().position = buttonPosition;
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
