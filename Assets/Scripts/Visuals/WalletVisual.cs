using TMPro;
using UnityEngine;

public class WalletVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text walletText;

    public void UpdateWalletAmount(int amount)
    {
        walletText.text = amount.ToString() + " €";
    }
}
