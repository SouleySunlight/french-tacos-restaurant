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
        UpdateModal(hideButton);
        tutorialMessageModal.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, yPosition);
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

    void UpdateModal(bool hideButton)
    {
        var rectTransform = tutorialMessageModal.GetComponent<RectTransform>();
        var textRectTransform = messageText.GetComponent<RectTransform>();

        var offsetMin = textRectTransform.offsetMin;
        offsetMin.y = hideButton ? 30 : 200;
        textRectTransform.offsetMin = offsetMin;

        var size = rectTransform.sizeDelta;
        size.y = hideButton ? 400 : 500;
        rectTransform.sizeDelta = size;
    }
}