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
}
