using UnityEngine;

public class RewardButtonBehavior : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.CompletionBarManager.OnClickOnRewardModalButton();
    }
}