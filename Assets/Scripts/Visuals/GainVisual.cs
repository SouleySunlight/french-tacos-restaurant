using System.Collections.Generic;
using UnityEngine;

public class GainVisual : MonoBehaviour
{
    [SerializeField] private GameObject gainPrefab;

    private List<GameObject> visibleGains = new();

    public void CreateNewGain(Sprite sprite, int quantity)
    {
        var createdGain = Instantiate(gainPrefab, this.transform);
        var gainDisplayer = createdGain.GetComponent<GainDisplayer>();
        gainDisplayer.gainSprite = sprite;
        gainDisplayer.quantity = quantity;
        gainDisplayer.UpdateVisual();
        PositionGain(createdGain);

        visibleGains.Add(createdGain);
    }

    void PositionGain(GameObject gain)
    {
        var totalWidth = GetComponent<RectTransform>().rect.width;
        var rect = gain.GetComponent<RectTransform>();

        var horizontalOffset = 65;
        var verticalOffset = 25;

        var xPosition = horizontalOffset + Random.Range(0, totalWidth - horizontalOffset);

        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(xPosition, verticalOffset);

    }
}