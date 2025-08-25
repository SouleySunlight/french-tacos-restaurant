using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageButtonDisplayer : MonoBehaviour
{
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Sprite onBackground;
    [SerializeField] private Sprite offBackground;
    [SerializeField] private string localeCode;

    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (LocalizationSettings.SelectedLocale.Identifier.Code == localeCode)
        {
            buttonBackground.sprite = onBackground;
            return;
        }
        buttonBackground.sprite = offBackground;
    }

    public void OnClick()
    {
        if (LocalizationSettings.SelectedLocale.Identifier.Code == localeCode)
        {
            return;
        }
        var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
        UpdateVisual();

    }
}