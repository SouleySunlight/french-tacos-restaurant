using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainVisual : MonoBehaviour
{
    [SerializeField] private GameObject gainPrefab;

    public void CreateNewGain(Sprite sprite, int quantity)
    {
        var createdGain = Instantiate(gainPrefab, this.transform);
        var gainDisplayer = createdGain.GetComponent<GainDisplayer>();
        gainDisplayer.gainSprite = sprite;
        gainDisplayer.quantity = quantity;
        gainDisplayer.UpdateVisual();
        PositionGain(createdGain);

        var animationTime = Random.Range(10, 20);
        StartCoroutine(ElevateGainCoroutine(createdGain, animationTime));
    }

    void PositionGain(GameObject gain)
    {
        var totalWidth = GetComponent<RectTransform>().rect.width;
        var rect = gain.GetComponent<RectTransform>();

        var horizontalOffset = 115;
        var verticalOffset = 50;

        var xPosition = horizontalOffset + Random.Range(0, totalWidth - 2 * horizontalOffset);

        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(xPosition, verticalOffset);

    }

    IEnumerator ElevateGainCoroutine(GameObject gain, float time)
    {
        var rectTransform = gain.GetComponent<RectTransform>();
        Vector3 startingPos = rectTransform.position;
        float finalHeight = GetComponent<RectTransform>().rect.height + 200;
        Vector3 finalPos = new(startingPos.x, finalHeight, 0);
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float animationCompleteRatio = elapsedTime / time;
            var position = Vector3.Lerp(startingPos, finalPos, animationCompleteRatio);
            position.y = startingPos.y + (finalPos.y - startingPos.y) * animationCompleteRatio;
            rectTransform.position = position;
            var opacity = 1 - 0.5f * animationCompleteRatio;
            gain.GetComponent<GainDisplayer>().UpdateOpacity(opacity);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gain);
    }
}