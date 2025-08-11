using TMPro;
using UnityEngine;

public class WalletVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text walletText;

    public void UpdateWalletAmount(float amount)
    {

        walletText.text = FormatAmount(amount);
    }

    string FormatAmount(float amount)
    {
        string[] suffixes = { "", "K", "M", "B", "Q" };
        int suffixIndex = 0;

        while (amount >= 1000f && suffixIndex < suffixes.Length - 1)
        {
            amount /= 1000f;
            suffixIndex++;
        }

        float factor = Mathf.Pow(10, Mathf.Max(0, 3 - (int)Mathf.Floor(amount).ToString().Length));
        amount = Mathf.Floor(amount * factor) / factor;

        return amount.ToString("0.##") + suffixes[suffixIndex];
    }
}
