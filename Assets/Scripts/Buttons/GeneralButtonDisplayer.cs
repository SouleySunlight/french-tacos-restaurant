using UnityEngine;

public class GeneralButtonDisplayer : MonoBehaviour
{
    [SerializeField] private RectTransform buttonTransform;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float verticalMovement = 0f;

    public void OnPressDown()
    {
        var newPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y - verticalMovement);
        buttonTransform.anchoredPosition = newPosition;
        shadow.SetActive(false);
    }

    public void OnRelease()
    {
        var newPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y + verticalMovement);
        buttonTransform.anchoredPosition = newPosition;
        shadow.SetActive(true);
    }
}