using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class WorkersButtonDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject buttonBody;
    [SerializeField] private GameObject shadow;
    [SerializeField] private TMP_Text buttonText;

    public void UpdateVisual()
    {
        if (GameManager.Instance.DayCycleManager.GetCurrentDay() < 2)
        {
            gameObject.SetActive(false);
            return;
        }
        var currentView = PlayzoneVisual.currentView;

        if (currentView == ViewToShowEnum.TACOS_MAKER)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
    }
    public void OnClick()
    {
        GameManager.Instance.WorkersManager.ShowWorkerModal();
    }

    public void OnRelease()
    {
        var rectTransform = buttonBody.GetComponent<RectTransform>();
        var newPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 15f);
        rectTransform.anchoredPosition = newPosition;
        shadow.SetActive(true);
    }

    public void UpdateButtonText(bool hasWorkerActive)
    {
        var key = hasWorkerActive ? "WORKERS.ACTIVE" : "WORKERS.INACTIVE";
        buttonText.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", key);
    }
}