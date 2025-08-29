
using UnityEngine;
using UnityEngine.UI;

public class SidebarCompletionBar : MonoBehaviour
{

    [SerializeField] private Image completionBar;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip doneSound;
    private bool isTimerOver = false;


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
        if (percentage >= 1)
        {
            animator.SetBool("isTimerOver", true);
            if (!isTimerOver)
            {
                GameManager.Instance.SoundManager.PlaySFX(doneSound);
                isTimerOver = true;
            }
        }
        if (percentage < 1)
        {
            animator.SetBool("isTimerOver", false);
            isTimerOver = false;

        }
    }
}