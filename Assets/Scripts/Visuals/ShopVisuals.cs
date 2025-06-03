using System.Collections.Generic;
using UnityEngine;

public class ShopVisuals : MonoBehaviour, IView
{
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private Transform firstButtonPosition;

    private List<GameObject> buttons = new();


    public void SetupIngredientToBuy(List<Ingredient> ingredients)
    {

        foreach (Ingredient ingredient in ingredients)
        {
            var buttonPrefab = Instantiate(ingredientButtonPrefab, firstButtonPosition.position, Quaternion.identity, firstButtonPosition);
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => GameManager.Instance.UnlockIngredient(ingredient));
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().shouldShowPrice = true;
            buttonPrefab.GetComponent<IngredientButtonDisplayer>().shouldShowQuantity = false;

            buttons.Add(buttonPrefab);
        }
        UpdateVisual();
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        var buttonToRemove = buttons.Find(button => button.GetComponent<IngredientButtonDisplayer>().ingredientData.id == ingredient.id);
        Destroy(buttonToRemove);
        buttons.Remove(buttonToRemove);
        UpdateVisual();
    }

    void UpdateVisual()
    {
        var index = 0;
        foreach (var button in buttons)
        {
            var buttonPosition = new Vector3(
                           firstButtonPosition.position.x,
                           firstButtonPosition.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * index,
                           firstButtonPosition.position.z
                       );
            button.GetComponent<RectTransform>().position = buttonPosition;
            index++;
        }
    }
}