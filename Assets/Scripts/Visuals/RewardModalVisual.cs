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
    [SerializeField] private AudioClip rewardSound;


    public void ShowRewardModal()
    {
        GameManager.Instance.isGamePaused = true;
        GameManager.Instance.SoundManager.PlaySFX(rewardSound);
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
        if (reward.rewardType == RewardType.INCREASE_MAX_INGREDIENTS)
        {
            ShowMaxIngredientsRewardModal();
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
        previousValue.text = GameManager.Instance.OrdersManager.GetTacosPrice().ToString();
        newValue.text = (GameManager.Instance.OrdersManager.GetTacosPrice() + 1).ToString();
        quantityDisplay.SetActive(true);
    }

    void ShowOrderRewardModal()
    {
        rewardTitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.ORDER");
        rewardSubtitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.ORDER_SUBTITLE");
        image.gameObject.SetActive(false);
        previousValue.text = GameManager.Instance.OrdersManager.GetMaxNumberOfOrders().ToString();
        newValue.text = (GameManager.Instance.OrdersManager.GetMaxNumberOfOrders() + 1).ToString();
        quantityDisplay.SetActive(true);
    }

    void ShowMaxIngredientsRewardModal()
    {
        rewardTitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.MAX_INGREDIENT");
        rewardSubtitle.text = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "REWARD.MAX_INGREDIENT_SUBTITLE");
        image.gameObject.SetActive(false);
        previousValue.text = GameManager.Instance.InventoryManager.GetProcessedIngredientMaxAmount().ToString();
        newValue.text = (GameManager.Instance.InventoryManager.GetProcessedIngredientMaxAmount() + 5).ToString();
        quantityDisplay.SetActive(true);
    }
}