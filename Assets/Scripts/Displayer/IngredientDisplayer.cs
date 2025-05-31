using UnityEngine;
using UnityEngine.UI;

public class IngredientDisplayer : MonoBehaviour
{
    [SerializeField] private Image ingredientImage;
    public Ingredient ingredientData;

    void Start()
    {
        UpdateIngredientVisual();
    }

    void UpdateIngredientVisual()
    {
        if (PlayzoneVisual.currentView == ViewToShowEnum.TACOS_MAKER)
        {
            DisplayInTacosImage();
            return;
        }

        DisplayUnprocessedImage();
    }

    public void DisplayInTacosImage()
    {
        ingredientImage.sprite = ingredientData.processedSprite;
        UseTacosSize();
    }

    public void DisplayUnprocessedImage()
    {
        ingredientImage.sprite = ingredientData.unprocessedSprite;
        UseCommonSize();
    }

    void UseTacosSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(GlobalConstant.INGREDIENT_IN_TACOS_IMAGE_DIMENSION, GlobalConstant.INGREDIENT_IN_TACOS_IMAGE_DIMENSION);
    }

    void UseCommonSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(GlobalConstant.INGREDIENT_IMAGE_DIMENSION, GlobalConstant.INGREDIENT_IMAGE_DIMENSION);
    }


}
