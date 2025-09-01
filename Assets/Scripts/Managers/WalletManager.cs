using UnityEngine;

public class WalletManager : MonoBehaviour
{
    private WalletVisual walletVisual;

    private float currentWalletAmount = 0;
    public float moneyEarnedThisDay { get; private set; } = 0;
    public float moneySpentOnIngredientsThisDay { get; private set; } = 0;
    public float moneySpentOnWorkersThisDay { get; private set; } = 0;
    public float moneySpentOnUpgradeThisDay { get; private set; } = 0;
    [SerializeField] private AudioClip moneyReceivedSound;
    [SerializeField] private Sprite moneySprite;




    void Awake()
    {
        walletVisual = FindFirstObjectByType<WalletVisual>();
        walletVisual.UpdateWalletAmount(currentWalletAmount);
    }

    public void ReceiveMoney(float amount)
    {
        GameManager.Instance.SoundManager.PlaySFX(moneyReceivedSound);
        currentWalletAmount += amount;
        moneyEarnedThisDay += amount;
        walletVisual.UpdateWalletAmount(currentWalletAmount);
        GameManager.Instance.GainManager.CreateNewGain(moneySprite, Mathf.RoundToInt(amount));

    }

    public float GetCurrentAmount()
    {
        return currentWalletAmount;
    }
    public void SetCurrentAmount(float amount)
    {
        currentWalletAmount = amount;
        walletVisual.UpdateWalletAmount(currentWalletAmount);
    }

    public bool HasEnoughMoney(float amount)
    {
        if (currentWalletAmount < amount)
        {
            GameManager.Instance.HelpTextManager.ShowNotEnoughGoldMessage();
        }
        return currentWalletAmount >= amount;
    }

    public void SpendMoney(float amount, SpentCategoryEnum spentCategoryEnum)
    {
        currentWalletAmount -= amount;
        switch (spentCategoryEnum)
        {
            case SpentCategoryEnum.INGREDIENTS:
                moneySpentOnIngredientsThisDay += amount;
                break;
            case SpentCategoryEnum.WORKERS:
                moneySpentOnWorkersThisDay += amount;
                break;
            case SpentCategoryEnum.UPGRADE:
                moneySpentOnUpgradeThisDay += amount;
                break;
        }
        walletVisual.UpdateWalletAmount(currentWalletAmount);
    }

    public void ResetDailyCount()
    {
        moneyEarnedThisDay = 0;
        moneySpentOnIngredientsThisDay = 0;
        moneySpentOnUpgradeThisDay = 0;
        moneySpentOnWorkersThisDay = 0;
    }

    public float GetMoneySpentThisDay()
    {
        return moneySpentOnIngredientsThisDay + moneySpentOnWorkersThisDay + moneySpentOnUpgradeThisDay;
    }
}
