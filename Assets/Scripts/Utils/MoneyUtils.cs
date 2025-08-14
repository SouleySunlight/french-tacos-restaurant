using UnityEngine;

public static class MoneyUtils
{
    public static string FormatAmount(float amount)
    {
        string[] suffixes = { "", "K", "M", "B", "Q" };
        int suffixIndex = 0;
        float absAmount = Mathf.Abs(amount);

        while (absAmount >= 1000f && suffixIndex < suffixes.Length - 1)
        {
            absAmount /= 1000f;
            suffixIndex++;
        }

        int intDigits = Mathf.FloorToInt(absAmount).ToString().Length;
        float factor = Mathf.Pow(10, Mathf.Max(0, 3 - intDigits));
        absAmount = Mathf.Floor(absAmount * factor) / factor;

        if (amount < 0)
            absAmount *= -1;

        return absAmount.ToString("0.##") + suffixes[suffixIndex];
    }
}