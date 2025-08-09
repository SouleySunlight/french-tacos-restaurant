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
        if (PlayzoneVisual.currentView == ViewToShowEnum.TACOS_MAKER || PlayzoneVisual.currentView == ViewToShowEnum.SAUCE_GRUYERE)
        {
            DisplayInTacosImage();
            return;
        }

        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER)
        {
            DisplayUnprocessedInFryerImage();
            ingredientImage.raycastTarget = false;
            return;
        }

        DisplayUnprocessedImage();
    }

    public void DisplayInTacosImage()
    {
        var spriteToShow = ingredientData.inTacosSprite;
        if (ingredientData.category == IngredientCategoryEnum.SAUCE)
        {
            spriteToShow = GameManager.Instance.TacosMakerManager.GetNumberOfSauceOfOnCreationTacos() % 2 == 0 ? ingredientData.inTacosSpriteAlternative : ingredientData.inTacosSprite;
        }
        ingredientImage.sprite = spriteToShow;
        if (PlayzoneVisual.currentView == ViewToShowEnum.SAUCE_GRUYERE)
        {
            UseGruyereSize();
            return;

        }
        UseTacosSize();
    }

    public void DisplayUnprocessedInFryerImage()
    {
        ingredientImage.sprite = ingredientData.uncookedFryerSprite;
        UseFryerSize();
    }
    public void DisplayProcessedInFryerImage()
    {
        ingredientImage.sprite = ingredientData.cookedFryerSprite;
        UseFryerSize();
    }

    public void DisplayWastedInFryerImage()
    {
        ingredientImage.sprite = ingredientData.burntFryerSprite;
        UseFryerSize();
    }


    public void DisplayUnprocessedImage()
    {
        ingredientImage.sprite = ingredientData.unprocessedSprite;
        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER || PlayzoneVisual.currentView == ViewToShowEnum.SAUCE_GRUYERE)
        {
            UseLargeSize();
            return;
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.HOTPLATE)
        {
            UseMediumSize();
            return;
        }
        UseSmallSize();
    }
    public void DisplayProcessedImage()
    {
        ingredientImage.sprite = ingredientData.processedSprite;
        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER || PlayzoneVisual.currentView == ViewToShowEnum.SAUCE_GRUYERE)
        {
            UseLargeSize();
            return;
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.HOTPLATE)
        {
            UseMediumSize();
            return;
        }
        UseSmallSize();
    }

    public void DisplayWastedImage()
    {
        ingredientImage.sprite = ingredientData.wastedSprite;
        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER || PlayzoneVisual.currentView == ViewToShowEnum.SAUCE_GRUYERE)
        {
            UseLargeSize();
            return;
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.HOTPLATE)
        {
            UseMediumSize();
            return;
        }
        UseSmallSize();
    }

    void UseTacosSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(GlobalConstant.INGREDIENT_IN_TACOS_IMAGE_WIDTH, GlobalConstant.INGREDIENT_IN_TACOS_IMAGE_HEIGHT);
    }

    void UseLargeSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(GlobalConstant.INGREDIENT_FRYER_IMAGE_DIMENSION, GlobalConstant.INGREDIENT_FRYER_IMAGE_DIMENSION);
    }

    void UseSmallSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(GlobalConstant.INGREDIENT_IMAGE_DIMENSION, GlobalConstant.INGREDIENT_IMAGE_DIMENSION);
    }

    void UseMediumSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(150, 150);
    }

    void UseFryerSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(205, 350);

    }

    void UseGruyereSize()
    {
        ingredientImage.GetComponent<RectTransform>().sizeDelta = new(700, 700);


    }
}
