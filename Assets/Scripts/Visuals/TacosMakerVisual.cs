using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TacosMakerVisual : MonoBehaviour
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

    public void CreateTacos()
    {
        onCreationTacos = Instantiate(tortillaPrefab, onCreationTacosTransform.position, Quaternion.identity, onCreationTacosTransform);
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        var index = 0;
        foreach (Ingredient ingredient in ingredients)
        {
            var buttonPosition = new Vector3(
                ingredientButtonFirstTransform.position.x + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                ingredientButtonFirstTransform.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
                ingredientButtonFirstTransform.position.z
            );

            var buttonPrefab = Instantiate(ingredientButtonPrefab, buttonPosition, Quaternion.identity, ingredientButtonFirstTransform);
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => OnClickToAddIngredient(ingredient));
            buttons.Add(buttonPrefab);
            index++;
        }
    }

    void OnClickToAddIngredient(Ingredient ingredient)
    {
        if (onCreationTacos == null) { return; }

        GameManager.Instance.TacosMakerManager.AddIngredients(ingredient);
    }
    public void AddIngredient(Ingredient ingredient)
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
