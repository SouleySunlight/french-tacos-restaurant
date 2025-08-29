using TMPro;
using UnityEngine;

public class HelpTextVisual : MonoBehaviour
{
    [SerializeField] private GameObject helpTextDisplayer;

    public void ShowMessage(string messageKey, string parameter = null)
    {
        var createdText = Instantiate(helpTextDisplayer, this.transform);
        createdText.GetComponent<HelpTextDisplayer>().ShowMessage(messageKey, parameter);
        PositionMessage(createdText);

    }

    void PositionMessage(GameObject message)
    {
        var rect = message.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0f, 0.75f);
        rect.anchorMax = new Vector2(1, 0.75f);
        rect.anchoredPosition = new Vector2(0, 0);
    }
}