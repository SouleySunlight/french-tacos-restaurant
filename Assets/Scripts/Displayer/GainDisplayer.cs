using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GainDisplayer : MonoBehaviour
{
    public Sprite gainSprite;
    public int quantity;
    [SerializeField] TMP_Text quantityText;
    [SerializeField] Image image;

    public void UpdateVisual()
    {
        quantityText.text = "+" + quantity;
        image.sprite = gainSprite;
    }

    public void UpdateOpacity(float transparency)
    {
        Color imageColor = image.color;
        imageColor.a = transparency;
        image.color = imageColor;

        Color textColor = quantityText.color;
        textColor.a = transparency;
        quantityText.color = textColor;
    }
}