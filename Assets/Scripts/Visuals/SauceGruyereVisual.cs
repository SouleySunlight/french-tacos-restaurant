
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SauceGruyereVisual : MonoBehaviour, IView
{
    [SerializeField] private RectTransform firstIngredientPosition;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private RectTransform sauceGruyerePosition;
    [SerializeField] private Image cookingTimer;
    [SerializeField] private Ingredient sauceGruyereIngredient;


    private List<GameObject> buttons = new();
    private List<GameObject> ingredientsInSauceGruyere = new();
    private GameObject sauceGruyerePrefab = null;
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;

    public void OnViewDisplayed()
    {
        UpdateIngredientButtons();
    }


    void Awake()
    {
        CreateSauceGruyere();
        sauceGruyerePrefab.SetActive(false);

    }
    public void AddAvailableIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, firstIngredientPosition.position, Quaternion.identity, firstIngredientPosition);
        buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().ingredientData = ingredient;
        buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().shouldShowUnprocessedQuantity = true;


        buttonPrefab.GetComponent<LegacyIngredientButtonDisplayer>().AddListener(() => GameManager.Instance.SauceGruyereManager.AddIngredientToSauceGruyere(ingredient));
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
        ingredientToCook.GetComponent<IngredientMovement>().ClickSauceGruyereEvent.AddListener(OnClickOnIngredient);
        ingredientsInSauceGruyere.Add(ingredientToCook);
        UpdateIngredientButtons();
    }

    public void CreateSauceGruyere()
    {
        sauceGruyerePrefab = Instantiate(ingredientPrefab, sauceGruyerePosition.position, Quaternion.identity, sauceGruyerePosition);
        sauceGruyerePrefab.GetComponent<IngredientDisplayer>().ingredientData = sauceGruyereIngredient;
        sauceGruyerePrefab.GetComponent<IngredientMovement>().ClickSauceGruyereEvent.AddListener(OnClickOnIngredient);

    }

    public void UpdateIngredientButtons()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<LegacyIngredientButtonDisplayer>().GetComponent<LegacyIngredientButtonDisplayer>().UpdateVisual();
        }
    }

    public void UpdateTimer(float percentage)
    {
        cookingTimer.fillAmount = percentage;
    }

    public void OnSauceGruyereCooked()
    {
        RemoveIngredientsFromSauceGruyere();
        sauceGruyerePrefab.SetActive(true);
    }

    public void OnSauceGruyereBurnt()
    {
        sauceGruyerePrefab.GetComponent<IngredientDisplayer>().DisplayWastedImage();
    }


    public void RemoveIngredientsFromSauceGruyere()
    {
        foreach (var ingredient in ingredientsInSauceGruyere)
        {
            Destroy(ingredient);
        }
        ingredientsInSauceGruyere.Clear();
        UpdateIngredientButtons();
    }
    void OnClickOnIngredient(GameObject gameObject)
    {
        GameManager.Instance.SauceGruyereManager.OnClickOnIngredient(gameObject.GetComponent<IngredientDisplayer>().ingredientData);
    }

    public void RemoveSauceGruyere()
    {
        sauceGruyerePrefab.GetComponent<IngredientDisplayer>().DisplayProcessedImage();
        sauceGruyerePrefab.SetActive(false);
    }



}