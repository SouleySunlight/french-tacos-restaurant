using System.Collections.Generic;
using UnityEngine;

public class ShopVisuals : MonoBehaviour, IView
{
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private Transform firstButtonPosition;

    private List<GameObject> buttons = new();


    public void SetupIngredientToBuy(List<Ingredient> ingredients)
    {
        var index = 0;
        foreach (Ingredient ingredient in ingredients)
        {
            var buttonPosition = new Vector3(
                firstButtonPosition.position.x,
                firstButtonPosition.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * index,
                firstButtonPosition.position.z
            );

            var buttonPrefab = Instantiate(ingredientButtonPrefab, buttonPosition, Quaternion.identity, firstButtonPosition);
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().shouldShowPrice = true;
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().shouldShowQuantity = false;

            buttons.Add(buttonPrefab);
            index++;
        }
    }
}