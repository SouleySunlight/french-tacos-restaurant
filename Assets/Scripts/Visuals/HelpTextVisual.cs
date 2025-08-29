using TMPro;
using UnityEngine;

public class HelpTextVisual : MonoBehaviour
{
    [SerializeField] private GameObject helpTextDisplayer;
    private GameObject currentMessage = null;

    public void ShowMessage(string messageKey, string parameter = null)
    {
        var createdText = Instantiate(helpTextDisplayer, this.transform);
        createdText.GetComponent<HelpTextDisplayer>().ShowMessage(messageKey, parameter);
        DestroyExisitingMessage();
        PositionMessage(createdText);
        currentMessage = createdText;

    }

    void PositionMessage(GameObject message)
    {
        var rect = message.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0f, 0.75f);
        rect.anchorMax = new Vector2(1, 0.75f);
        rect.anchoredPosition = new Vector2(0, 0);
    }

    void DestroyExisitingMessage()
    {
        if (currentMessage == null) { return; }
        Destroy(currentMessage);
        currentMessage = null;
    }
}