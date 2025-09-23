using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class TutorialMessageDisplayer : MonoBehaviour
{
    [SerializeField] GameObject tutorialMessageModal;
    [SerializeField] TMP_Text messageText;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject background;

    public void ShowMessage(string messageKey, int yPosition = 0, bool hideButton = false, bool showBackground = false)
    {
        tutorialMessageModal.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPosition);
        background.SetActive(showBackground);
        tutorialMessageModal.SetActive(false);
        nextButton.SetActive(!hideButton);
        messageText.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", messageKey);
        tutorialMessageModal.SetActive(true);
    }

    public void HideMessage()
    {
        tutorialMessageModal.SetActive(false);
    }

    public void OnNextButtonClicked()
    {
        GameManager.Instance.TutorialManager.OnNextButtonClicked();
    }
}