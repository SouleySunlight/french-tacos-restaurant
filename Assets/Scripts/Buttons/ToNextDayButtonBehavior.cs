using UnityEngine;

public class ToNextDayButtonBehavior : MonoBehaviour
{

    [SerializeField] private RectTransform buttonTransform;
    [SerializeField] private GameObject shadow;

    public void OnPressDown()
    {
        var newPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y - 5f);
        buttonTransform.anchoredPosition = newPosition;
        shadow.SetActive(false);
    }

    public void OnRelease()
    {
        var newPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y + 5f);
        buttonTransform.anchoredPosition = newPosition;
        shadow.SetActive(true);
    }

    public void OnClick()
    {
        GameManager.Instance.DayCycleManager.ToNextDay();
    }
}