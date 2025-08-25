using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MusicButtonDisplayer : MonoBehaviour
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
        if (SoundManager.isMusicOn)
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
        if (SoundManager.isMusicOn)
        {
            GameManager.Instance.SoundManager.TurnOffMusic();
            UpdateVisual();
            GameManager.Instance.SaveSettings();

            return;
        }
        GameManager.Instance.SoundManager.TurnOnMusic();
        UpdateVisual();
        GameManager.Instance.SaveSettings();

    }

}