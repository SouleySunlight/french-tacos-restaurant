using System.Collections;
using TMPro;
using UnityEngine;

public class HelpTextVisual : MonoBehaviour
{
    [SerializeField] private GameObject helpTextDisplayer;
    private GameObject currentMessage = null;
    private Coroutine coroutine = null;

    public void ShowMessage(string messageKey, string parameter = null)
    {
        var createdText = Instantiate(helpTextDisplayer, this.transform);
        createdText.GetComponent<HelpTextDisplayer>().ShowMessage(messageKey, parameter);
        DestroyExisitingMessage();
        PositionMessage(createdText);
        currentMessage = createdText;
        coroutine = StartCoroutine(ElevateMessage(currentMessage, 3));

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
        StopCoroutine(coroutine);
        currentMessage = null;
        coroutine = null;
    }

    IEnumerator ElevateMessage(GameObject message, float time)
    {
        var rectTransform = message.GetComponent<RectTransform>();
        Vector3 startingPos = rectTransform.position;
        float finalHeight = startingPos.y + 100;
        Vector3 finalPos = new(startingPos.x, finalHeight, 0);
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float animationCompleteRatio = elapsedTime / time;
            var position = Vector3.Lerp(startingPos, finalPos, animationCompleteRatio);
            position.y = startingPos.y + (finalPos.y - startingPos.y) * animationCompleteRatio;
            rectTransform.position = position;
            var opacity = 1 - 0.5f * animationCompleteRatio;
            message.GetComponent<HelpTextDisplayer>().UpdateOpacity(opacity);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(currentMessage);
    }
}