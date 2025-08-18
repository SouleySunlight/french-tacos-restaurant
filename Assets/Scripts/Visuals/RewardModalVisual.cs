using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class RewardModalVisual : MonoBehaviour
{
    [SerializeField] private GameObject rewardModal;
    [SerializeField] private LocalizeStringEvent rewardTitle;
    [SerializeField] private LocalizeStringEvent rewardSubtitle;
    [SerializeField] private Image image;
    [SerializeField] private GameObject quantityDisplay;
    [SerializeField] private TMP_Text previousValue;
    [SerializeField] private TMP_Text newValue;


    public void ShowRewardModal(Reward reward)
    {
        if (reward.rewardType == RewardType.UNLOCK_INGREDIENT)
        {
            ShowIngredientRewardModal(reward.ingredientToUnlock);
        }
        if (reward.rewardType == RewardType.PRICE_INCREASE)
        {
            ShowTacosPriceRewardModal();
        }
        if (reward.rewardType == RewardType.MORE_ORDERS)
        {
            ShowOrderRewardModal();
        }
        rewardModal.SetActive(true);
    }

    public void HideRewardModal()
    {
        rewardModal.SetActive(false);
    }

    void ShowIngredientRewardModal(Ingredient ingredient)
    {
        quantityDisplay.SetActive(false);
        rewardTitle.StringReference.TableEntryReference = "REWARD.INGREDIENT";
        rewardSubtitle.StringReference.TableEntryReference = "INGREDIENT_" + ingredient.id.ToUpper();
        image.sprite = ingredient.processedSprite;
        image.gameObject.SetActive(true);
    }

    void ShowTacosPriceRewardModal()
    {
        rewardTitle.StringReference.TableEntryReference = "REWARD.TACOS";
        rewardSubtitle.StringReference.TableEntryReference = "REWARD.TACOS_SUBTITLE";
        image.gameObject.SetActive(false);
        previousValue.text = (GameManager.Instance.OrdersManager.GetTacosPrice() - 1).ToString();
        newValue.text = GameManager.Instance.OrdersManager.GetTacosPrice().ToString();
        quantityDisplay.SetActive(true);
    }

    void ShowOrderRewardModal()
    {
        rewardTitle.StringReference.TableEntryReference = "REWARD.ORDER";
        rewardSubtitle.StringReference.TableEntryReference = "REWARD.ORDER_SUBTITLE";
        image.gameObject.SetActive(false);
        previousValue.text = (GameManager.Instance.OrdersManager.GetMaxNumberOfOrders() - 1).ToString();
        newValue.text = GameManager.Instance.OrdersManager.GetMaxNumberOfOrders().ToString();
        quantityDisplay.SetActive(true);
    }
}