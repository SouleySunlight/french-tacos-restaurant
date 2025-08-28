
using UnityEngine;
using UnityEngine.UI;

public class RoundedCompletionBarDisplayer : MonoBehaviour
{

    [SerializeField] private Image completionBar;
    [SerializeField] private Animator animator;

    public void UpdateTimer(float percentage)
    {
        completionBar.fillAmount = percentage;

        if (percentage == 0)
        {
            animator.SetBool("isTimerOver", false);
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        if (percentage >= 1)
        {
            animator.SetBool("isTimerOver", true);
        }
    }
}