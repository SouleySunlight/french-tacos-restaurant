using TMPro;
using UnityEngine;

public class QuantityDisplayer : MonoBehaviour
{
    public int currentQuantity;
    [SerializeField] private GameObject quantityDisplayer;

    public void UpdateVisual()
    {
        var text = quantityDisplayer.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = "x" + currentQuantity;
        }
        if (currentQuantity > 0)
        {
            quantityDisplayer.SetActive(true);
            return;
        }
        quantityDisplayer.SetActive(false);
    }

    public void SetQuantity(int quantity)
    {
        currentQuantity = quantity;
        UpdateVisual();
    }
}