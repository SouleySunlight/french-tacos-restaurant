using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class RewardModalVisual : MonoBehaviour
{
    [SerializeField] private GameObject rewardModal;
    [SerializeField] private TMP_Text rewardTitle;
    [SerializeField] private TMP_Text rewardSubtitle;
    [SerializeField] private Image image;
    [SerializeField] private GameObject quantityDisplay;
    [SerializeField] private TMP_Text previousValue;
    [SerializeField] private TMP_Text newValue;


    public void ShowRewardModal()
    {
        GameManager.Instance.isGamePaused = true;
        rewardModal.SetActive(true);
    }

    public void LoadNextRewardModal(Reward reward)
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
    }

    public void HideRewardModal()
    {
        GameManager.Instance.isGamePaused = false;
        rewardModal.SetActive(false);
    }

    void ShowIngredientRewardModal(Ingredient ingredient)
    {
        quantityDisplay.SetActive(false);
        rewardTitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.INGREDIENT");
        rewardSubtitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "INGREDIENT_" + ingredient.id.ToUpper());
        image.sprite = ingredient.processedSprite;
        image.gameObject.SetActive(true);
    }

    void ShowTacosPriceRewardModal()
    {
        rewardTitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.TACOS");
        rewardSubtitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.TACOS_SUBTITLE");
        image.gameObject.SetActive(false);
        previousValue.text = (GameManager.Instance.OrdersManager.GetTacosPrice() - 1).ToString();
        newValue.text = GameManager.Instance.OrdersManager.GetTacosPrice().ToString();
        quantityDisplay.SetActive(true);
    }

    void ShowOrderRewardModal()
    {
        rewardTitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.ORDER");
        rewardSubtitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.ORDER_SUBTITLE");
        image.gameObject.SetActive(false);
        previousValue.text = (GameManager.Instance.OrdersManager.GetMaxNumberOfOrders() - 1).ToString();
        newValue.text = GameManager.Instance.OrdersManager.GetMaxNumberOfOrders().ToString();
        quantityDisplay.SetActive(true);
    }
}