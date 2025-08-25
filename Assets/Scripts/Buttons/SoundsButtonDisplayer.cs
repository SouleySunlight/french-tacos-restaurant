using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SoundsButtonDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Sprite onBackground;
    [SerializeField] private Sprite offBackground;

    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (SoundManager.areSoundsOn)
        {
            buttonBackground.sprite = onBackground;
            buttonText.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "SETTINGS.ON");
            return;
        }
        buttonBackground.sprite = offBackground;
        buttonText.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "SETTINGS.OFF");
    }

    public void OnClick()
    {
        if (SoundManager.areSoundsOn)
        {
            GameManager.Instance.SoundManager.TurnOffSounds();
            UpdateVisual();

            return;
        }
        GameManager.Instance.SoundManager.TurnOnSounds();
        UpdateVisual();

    }

}