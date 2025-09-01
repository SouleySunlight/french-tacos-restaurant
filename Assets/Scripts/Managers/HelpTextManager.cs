using UnityEngine;
using UnityEngine.Localization.Settings;

public class HelpTextManager : MonoBehaviour
{
    private HelpTextVisual helpTextVisual;
    [SerializeField] private AudioClip errorAudio;

    void Awake()
    {
        helpTextVisual = FindFirstObjectByType<HelpTextVisual>(FindObjectsInactive.Include);
    }

    public void ShowMessage(string messageKey, string parameter = null)
    {
        helpTextVisual.ShowMessage(messageKey, parameter);
    }

    public void ShowBurntMessage(Ingredient ingredient)
    {
        var ingredientName = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "INGREDIENT_" + ingredient.id);

        ShowMessage("INGREDIENT.BURNT", ingredientName);
    }
    public void ShowBurntTacosMessage()
    {

        ShowMessage("INGREDIENT.BURNT", "Tacos");
    }

    public void ShowNotEnoughPlaceMessage()
    {
        GameManager.Instance.SoundManager.PlaySFX(errorAudio);
        ShowMessage("MESSAGE.NOT_ENOUGH_SPACE");
    }

    public void ShowNotEnoughIngredientMessage(Ingredient ingredient)
    {

        var ingredientName = LocalizationSettings.StringDatabase
             .GetLocalizedString("UI_Texts", "INGREDIENT_" + ingredient.id);

        GameManager.Instance.SoundManager.PlaySFX(errorAudio);
        ShowMessage("INGREDIENT.NOT_ENOUGH", ingredientName);
    }
    public void ShowNotEnoughGoldMessage()
    {
        GameManager.Instance.SoundManager.PlaySFX(errorAudio);
        ShowMessage("GOLD.NOT_ENOUGH");
    }
    public void ShowNotEnoughSpaceToAddIngredient()
    {
        GameManager.Instance.SoundManager.PlaySFX(errorAudio);
        ShowMessage("INGREDIENT.NOT_ENOUGH_SPACE_TO_ADD");
    }
    public void ShowWrongTacosMessage()
    {
        GameManager.Instance.SoundManager.PlaySFX(errorAudio);
        ShowMessage("ORDER.WRONG_TACOS");
    }
}