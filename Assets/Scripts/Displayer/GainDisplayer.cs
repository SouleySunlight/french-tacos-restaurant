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
}