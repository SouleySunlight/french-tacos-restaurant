using UnityEngine;
using UnityEngine.Localization.Settings;

public class HelpTextManager : MonoBehaviour
{
    private HelpTextVisual helpTextVisual;

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

        ShowMessage("MESSAGE.NOT_ENOUGH_SPACE");
    }

}