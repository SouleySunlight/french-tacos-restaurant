using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class HelpTextDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    public void ShowMessage(string messageKey, string parameter = null)
    {
        var message = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", messageKey);
        if (parameter != null)
        {
            message = string.Format(message, parameter);
        }
        messageText.text = message;
    }
}