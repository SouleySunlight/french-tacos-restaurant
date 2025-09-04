using UnityEngine;

public class DoubleGoldButtonDisplayer : MonoBehaviour
{
    [SerializeField] private RewardedAdTypeEnum rewardedAdTypeEnum;
    public void OnClick()
    {
        GameManager.Instance.AdsManager.ShowRewardedAd(RewardedAdTypeEnum.DOUBLE_GOLD);
    }
}