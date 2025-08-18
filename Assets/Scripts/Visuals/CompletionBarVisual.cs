using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletionBarVisual : MonoBehaviour
{
    [SerializeField] private Image completionBarImage;
    [SerializeField] private TMP_Text completionText;

    public void UpdateVisual(int currentNumberOfTacos, int target)
    {
        completionBarImage.fillAmount = (float)currentNumberOfTacos / target;
        completionText.text = $"{currentNumberOfTacos}/{target}";
    }

    public void ShowMaximum()
    {
        completionBarImage.fillAmount = 1f;
        completionText.text = "MAX";
    }
}