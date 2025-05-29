using UnityEngine;

public class WalletManager : MonoBehaviour
{
    private WalletVisual walletVisual;

    private int currentWalletAmount = 0;

    void Awake()
    {
        walletVisual = FindFirstObjectByType<WalletVisual>();
        walletVisual.UpdateWalletAmount(currentWalletAmount);
    }

    public void ReceiveMoney(int amount)
    {
        currentWalletAmount += amount;
        walletVisual.UpdateWalletAmount(currentWalletAmount);

    }
}
