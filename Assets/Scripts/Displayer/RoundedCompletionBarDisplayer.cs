
using UnityEngine;
using UnityEngine.UI;

public class RoundedCompletionBarDisplayer : MonoBehaviour
{

    [SerializeField] private Image completionBar;

    public void UpdateTimer(float percentage)
    {
        completionBar.fillAmount = percentage;

        if (percentage == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
    }
}