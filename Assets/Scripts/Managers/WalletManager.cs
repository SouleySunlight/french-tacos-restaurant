using UnityEngine;

public class WalletManager : MonoBehaviour
{
    private WalletVisual walletVisual;

    private float currentWalletAmount = 0;
    public float moneyEarnedThisDay { get; private set; } = 0;
    public float moneySpendThisDay { get; private set; } = 0;


    void Awake()
    {
        walletVisual = FindFirstObjectByType<WalletVisual>();
        walletVisual.UpdateWalletAmount(currentWalletAmount);
    }

    public void ReceiveMoney(float amount)
    {
        currentWalletAmount += amount;
        moneyEarnedThisDay += amount;
        walletVisual.UpdateWalletAmount(currentWalletAmount);
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
        return currentWalletAmount >= amount;
    }

    public void SpendMoney(float amount)
    {
        currentWalletAmount -= amount;
        moneySpendThisDay += amount;
        walletVisual.UpdateWalletAmount(currentWalletAmount);
    }

    public void ResetDailyCount()
    {
        moneyEarnedThisDay = 0;
        moneySpendThisDay = 0;
    }
}
