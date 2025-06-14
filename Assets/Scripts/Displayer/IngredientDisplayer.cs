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
        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER)
        {
            UseFryerSize();
            return;
        }
        UseCommonSize();
    }
    public void DisplayProcessedImage()
    {
        ingredientImage.sprite = ingredientData.processedSprite;
        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER)
        {
            UseFryerSize();
            return;
        }
        UseCommonSize();
    }

    public void DisplayWastedImage()
    {
        ingredientImage.sprite = ingredientData.wastedSprite;
        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER)
        {
            UseFryerSize();
            return;
        }
        UseCommonSize();
    }

    void UseTacosSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(GlobalConstant.INGREDIENT_IN_TACOS_IMAGE_DIMENSION, GlobalConstant.INGREDIENT_IN_TACOS_IMAGE_DIMENSION);
    }

    void UseFryerSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(GlobalConstant.INGREDIENT_FRYER_IMAGE_DIMENSION, GlobalConstant.INGREDIENT_FRYER_IMAGE_DIMENSION);
    }

    void UseCommonSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(GlobalConstant.INGREDIENT_IMAGE_DIMENSION, GlobalConstant.INGREDIENT_IMAGE_DIMENSION);
    }


}
