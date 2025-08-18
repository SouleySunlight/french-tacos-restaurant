using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class RewardModalVisual : MonoBehaviour
{
    [SerializeField] private GameObject rewardModal;
    [SerializeField] private LocalizeStringEvent rewardTitle;
    [SerializeField] private LocalizeStringEvent rewardSubtitle;
    [SerializeField] private Image image;

    public void ShowRewardModal(Reward reward)
    {
        if (reward.rewardType == RewardType.UNLOCK_INGREDIENT)
        {
            ShowIngredientRewardModal(reward.ingredientToUnlock);
        }
        rewardModal.SetActive(true);
    }

    void ShowIngredientRewardModal(Ingredient ingredient)
    {
        rewardTitle.StringReference.TableEntryReference = "REWARD.INGREDIENT";
        rewardSubtitle.StringReference.TableEntryReference = "INGREDIENT_" + ingredient.id.ToUpper();
        image.sprite = ingredient.processedSprite;
    }
}