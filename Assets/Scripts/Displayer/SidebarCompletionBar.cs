
using UnityEngine;
using UnityEngine.UI;

public class SidebarCompletionBar : MonoBehaviour
{

    [SerializeField] private Image completionBar;
    [SerializeField] private Animator animator;

    public void UpdateTimer(float percentage)
    {
        completionBar.fillAmount = percentage;

        if (percentage == 0)
        {
            gameObject.SetActive(false);
            if (animator == null) { return; }
            animator.SetBool("isTimerOver", false);
            return;
        }
        gameObject.SetActive(true);
        if (percentage >= 1)
        {
            if (animator == null) { return; }
            animator.SetBool("isTimerOver", true);
        }
    }
}