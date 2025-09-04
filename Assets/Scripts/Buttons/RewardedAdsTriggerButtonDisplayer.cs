using UnityEngine;

public class RewardedAdsTriggerButtonDisplayer : MonoBehaviour
{
    [SerializeField] private RewardedAdTypeEnum rewardedAdTypeEnum;
    public void OnClick()
    {
        GameManager.Instance.AdsManager.ShowRewardedAd(rewardedAdTypeEnum);
    }
}