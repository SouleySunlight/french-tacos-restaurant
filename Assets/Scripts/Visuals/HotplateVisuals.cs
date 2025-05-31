using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotplateVisuals : MonoBehaviour
{
    [SerializeField] private RectTransform firstIngredientPosition;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private List<GameObject> ingredients;
    [SerializeField] private List<RectTransform> cookPositions = new();
    [SerializeField] private List<Image> cookingTimers = new();

    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;

    void Start()
    {
        for (int i = 0; i < GlobalConstant.MAX_COOKING_INGREDIENTS; i++)
        {
            ingredients.Add(null);
        }
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        var index = 0;
        foreach (Ingredient ingredient in ingredients)
        {
            var buttonPosition = new Vector3(
                firstIngredientPosition.position.x + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                firstIngredientPosition.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
                firstIngredientPosition.position.z
            );

            var buttonPrefab = Instantiate(ingredientButtonPrefab, buttonPosition, Quaternion.identity, firstIngredientPosition);
            buttonPrefab.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.HotplateManager.CookIngredients(ingredient));
            buttonPrefab.GetComponentInChildren<TMP_Text>().text = ingredient.name;
            index++;
        }
    }

    public void CookIngredients(Ingredient ingredient, int position)
    {
        var ingredientToCook = Instantiate(ingredientPrefab, cookPositions[position].position, Quaternion.identity, cookPositions[position]);
        ingredientToCook.GetComponent<IngredientDisplayer>().ingredientData = ingredient;
        ingredients[position] = ingredientToCook;
    }

    public void OnIngredientCooked(int position)
    {
        ingredients[position].GetComponent<IngredientDisplayer>().DisplayProcessedImage();
    }

    public void UpdateTimer(int index, float percentage)
    {
        cookingTimers[index].fillAmount = percentage;
    }
}
