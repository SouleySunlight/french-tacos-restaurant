
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SauceGruyereVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private Image cookingTimer;
    [SerializeField] private Image creamImage;
    [SerializeField] private Image cheeseImage;
    [SerializeField] private Image sauceGruyereImage;
    [SerializeField] private Image burntSauceGruyereImage;

    private List<GameObject> buttons = new();

    public void OnViewDisplayed()
    {
        UpdateIngredientButtons();
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
        UpdateVisual();
    }

    void UpdateVisual()
    {
        var totalWidth = GetComponent<RectTransform>().rect.width;
        UIPlacement.PlaceIngredientButtons(buttons, totalWidth);
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
        UpdateIngredientButtons();
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

    public void UpdateTimer(float percentage)
    {
        cookingTimer.fillAmount = percentage;
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
        UpdateIngredientButtons();
    }
    public void OnClickOnPot()
    {
        GameManager.Instance.SauceGruyereManager.OnClickOnPot();
    }

    public void RemoveSauceGruyere()
    {
        sauceGruyereImage.gameObject.SetActive(false);
        burntSauceGruyereImage.gameObject.SetActive(false);
    }



}