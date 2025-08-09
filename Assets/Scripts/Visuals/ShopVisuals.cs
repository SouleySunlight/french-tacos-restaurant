using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopVisuals : MonoBehaviour, IView
{
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private Transform firstButtonPosition;
    [SerializeField] private Button viewToggleButton;

    private List<GameObject> buttons = new();

    public void OnViewDisplayed()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<LegacyIngredientButtonDisplayer>().GetComponent<LegacyIngredientButtonDisplayer>().UpdateVisual();
        }
    }

    public void SetupIngredientToBuy(List<Ingredient> ingredients)
    {
        DestroyAllButtons();
        buttons.Clear();
        foreach (Ingredient ingredient in ingredients)
        {
            var buttonPrefab = Instantiate(ingredientButtonPrefab, firstButtonPosition.position, Quaternion.identity, firstButtonPosition);
            buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().ingredientData = ingredient;
            buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().AddListener(() => GameManager.Instance.UnlockIngredient(ingredient));
            buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().shouldShowUnlockPrice = true;

            buttons.Add(buttonPrefab);
        }
        UpdateVisual();
    }

    public void RemoveIngredientToBuy(Ingredient ingredient)
    {
        var buttonToRemove = buttons.Find(button => button.GetComponent<LegacyIngredientButtonDisplayer>().ingredientData.id == ingredient.id);
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
                           firstButtonPosition.position.y + GlobalConstant.LEGACY_INGREDIENT_BUTTON_VERTICAL_GAP * index,
                           firstButtonPosition.position.z
                       );
            button.GetComponent<RectTransform>().position = buttonPosition;
            index++;
        }
    }

    void DestroyAllButtons()
    {
        foreach (var button in buttons)
        {
            Destroy(button);
        }
    }

    public void UpdateIngredientButtonVisual(Ingredient ingredient)
    {
        buttons.Find(button => button.GetComponent<LegacyIngredientButtonDisplayer>().ingredientData.id == ingredient.id).GetComponent<LegacyIngredientButtonDisplayer>().UpdateVisual();
    }
}