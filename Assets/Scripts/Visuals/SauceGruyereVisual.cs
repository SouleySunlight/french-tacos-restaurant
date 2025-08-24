
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SauceGruyereVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientIndicatorPrefab;
    [SerializeField] private GameObject roundedCompletionBar;
    [SerializeField] private Image creamImage;
    [SerializeField] private Image cheeseImage;
    [SerializeField] private Image sauceGruyereImage;
    [SerializeField] private Image burntSauceGruyereImage;

    private List<GameObject> buttons = new();
    private List<GameObject> indicators = new();


    public void OnViewDisplayed()
    {
        UpdateIngredients();
        GameManager.Instance.SauceGruyereManager.ManageCookingSoundOnViewChanged();
    }


    void Awake()
    {
        CreateSauceGruyere();
        sauceGruyereImage.gameObject.SetActive(false);
        cheeseImage.gameObject.SetActive(false);
        creamImage.gameObject.SetActive(false);
        burntSauceGruyereImage.gameObject.SetActive(false);


    }
    public void AddAvailableIngredient(Ingredient ingredient)
    {
        var buttonPrefab = Instantiate(ingredientButtonPrefab, this.transform);
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().ingredientData = ingredient;
        buttonPrefab.GetComponent<IngredientButtonDisplayer>().SetShouldShowUnprocessedIngredient(true);


        buttonPrefab.GetComponent<IngredientButtonDisplayer>().AddListener(() => GameManager.Instance.SauceGruyereManager.AddIngredientToSauceGruyere(ingredient));
        buttons.Add(buttonPrefab);
        UpdateUIPositions();
    }

    void UpdateUIPositions()
    {
        var totalWidth = GetComponent<RectTransform>().rect.width;
        UIPlacement.PlaceIngredientButtons(buttons, totalWidth);
        UIPlacement.PlaceIngredientIndicators(indicators, totalWidth);
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            AddAvailableIngredient(ingredient);
        }
        UpdateUIPositions();
    }

    public void SetupIngredientIndicators(List<Ingredient> ingredients)
    {

        foreach (var ingredient in ingredients)
        {
            var indicator = Instantiate(ingredientIndicatorPrefab, this.transform);
            indicator.GetComponent<IngredientIndicatorDisplayer>().ingredientData = ingredient;
            indicator.GetComponent<IngredientIndicatorDisplayer>().UpdateVisual();
            indicators.Add(indicator);
        }
        UpdateUIPositions();

    }

    public void AddIngredientToSauceGruyere(Ingredient ingredient)
    {
        if (ingredient.id == "CRE")
        {
            creamImage.gameObject.SetActive(true);
            return;
        }

        if (ingredient.id == "GRU")
        {
            cheeseImage.gameObject.SetActive(true);
            return;
        }
        UpdateIngredients();
    }

    public void CreateSauceGruyere()
    {
        sauceGruyereImage.gameObject.SetActive(true);

    }

    public void UpdateIngredientButtons()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<IngredientButtonDisplayer>().UpdateVisual();
        }
    }

    void UpdateIngredientIndicators()
    {
        foreach (var indicator in indicators)
        {
            indicator.GetComponent<IngredientIndicatorDisplayer>().UpdateVisual();
        }
    }

    void UpdateIngredients()
    {
        UpdateIngredientButtons();
        UpdateIngredientIndicators();
    }

    public void UpdateTimer(float percentage)
    {
        roundedCompletionBar.GetComponent<RoundedCompletionBarDisplayer>().UpdateTimer(percentage);
    }

    public void OnSauceGruyereCooked()
    {
        RemoveIngredientsFromSauceGruyere();
        sauceGruyereImage.gameObject.SetActive(true);
    }

    public void OnSauceGruyereBurnt()
    {
        burntSauceGruyereImage.gameObject.SetActive(true);
    }


    public void RemoveIngredientsFromSauceGruyere()
    {
        creamImage.gameObject.SetActive(false);
        cheeseImage.gameObject.SetActive(false);
        UpdateIngredients();
    }
    public void OnClickOnPot()
    {
        GameManager.Instance.SauceGruyereManager.OnClickOnPot();
    }

    public void RemoveSauceGruyere()
    {
        RemoveIngredientsFromSauceGruyere();
        sauceGruyereImage.gameObject.SetActive(false);
        burntSauceGruyereImage.gameObject.SetActive(false);
        UpdateIngredients();
    }



}