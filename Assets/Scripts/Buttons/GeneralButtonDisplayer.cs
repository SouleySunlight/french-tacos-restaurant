using UnityEngine;

public class GeneralButtonDisplayer : MonoBehaviour
{
    [SerializeField] private RectTransform buttonTransform;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float verticalMovement = 0f;
    [SerializeField] private AudioClip buttonSound;

    public void OnPressDown()
    {
        var newPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y - verticalMovement);
        buttonTransform.anchoredPosition = newPosition;
        shadow.SetActive(false);
        GameManager.Instance.SoundManager.PlaySFX(buttonSound);
        Debug.Log("Play sound");

    }

    public void OnRelease()
    {
        var newPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y + verticalMovement);
        buttonTransform.anchoredPosition = newPosition;
        shadow.SetActive(true);
        Debug.Log("dont Play sound");

    }
}