
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SauceGruyereVisual : MonoBehaviour, IView
{
    [SerializeField] private RectTransform firstIngredientPosition;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private Image cookingTimer;
    [SerializeField] private Image creamImage;
    [SerializeField] private Image cheeseImage;
    [SerializeField] private Image sauceGruyereImage;
    [SerializeField] private Image burntSauceGruyereImage;

    private List<GameObject> buttons = new();
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;

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
                firstIngredientPosition.position.x + GlobalConstant.LEGACY_INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                firstIngredientPosition.position.y + GlobalConstant.LEGACY_INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
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