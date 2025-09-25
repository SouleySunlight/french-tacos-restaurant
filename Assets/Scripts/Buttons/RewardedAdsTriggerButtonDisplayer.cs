using UnityEngine;

public class RewardedAdsTriggerButtonDisplayer : MonoBehaviour
{
    [SerializeField] private RewardedAdTypeEnum rewardedAdTypeEnum;
    public void OnClick()
    {
        if (GameManager.Instance.TutorialManager.currentTutorialType == TutorialType.WORKER)
        {
            GameManager.Instance.WorkersManager.HireRandomWorker();
            return;
        }
        GameManager.Instance.AdsManager.ShowRewardedAd(rewardedAdTypeEnum);
    }

    public void OnClickOnCloseRetrieveGoldModal()
    {
        GameManager.Instance.WalletManager.HideNotEnoughMoneyModal();
    }
}