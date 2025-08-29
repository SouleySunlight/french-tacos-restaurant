
using UnityEngine;
using UnityEngine.UI;

public class RoundedCompletionBarDisplayer : MonoBehaviour
{

    [SerializeField] private Image completionBar;
    [SerializeField] private Animator animator;
    [SerializeField] private bool shouldShowAnimation = true;

    void Start()
    {
        UpdateTimer(0);
    }

    public void UpdateTimer(float percentage)
    {
        completionBar.fillAmount = percentage;
        UpdateTimerAnimation(percentage);

        if (percentage == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
    }

    void UpdateTimerAnimation(float percentage)
    {
        if (animator == null && animator.runtimeAnimatorController == null) { return; }
        if (percentage >= 1 && shouldShowAnimation)
        {
            animator.SetBool("isTimerOver", true);
        }
        if (percentage < 1)
        {
            animator.SetBool("isTimerOver", false);

        }
    }
}