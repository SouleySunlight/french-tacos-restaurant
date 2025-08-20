using UnityEngine;

public class WorkersButtonDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject buttonBody;
    [SerializeField] private GameObject shadow;


    public void UpdateVisual()
    {
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

    public void OnPressDown()
    {
        var rectTransform = buttonBody.GetComponent<RectTransform>();
        var newPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - 15f);
        rectTransform.anchoredPosition = newPosition;
        shadow.SetActive(false);
    }

    public void OnRelease()
    {
        var rectTransform = buttonBody.GetComponent<RectTransform>();
        var newPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 15f);
        rectTransform.anchoredPosition = newPosition;
        shadow.SetActive(true);
    }
}