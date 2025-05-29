using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotplateVisuals : MonoBehaviour
{
    [SerializeField] private RectTransform firstIngredientPosition;
    [SerializeField] private GameObject ingredientButtonPrefab;
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;



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
            buttonPrefab.GetComponentInChildren<TMP_Text>().text = ingredient.name;
            index++;
        }
    }
}
