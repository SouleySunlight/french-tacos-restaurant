using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public string id;
    public int baseCost = GlobalConstant.DEFAULT_UPGRADE_BASE_COST;
    public float costEvolutionFactor = GlobalConstant.DEFAULT_UPGRADE_COST_FACTOR;
    public int maxLevel = GlobalConstant.DEFAULT_UPGRADE_MAX_LEVEL;
    public float speedMultiplier = GlobalConstant.DEFAULT_SPEED_UPGRADE;

    public int GetCostAtLevel(int level)
        => MoneyUtils.TruncateTo3Significant(Mathf.RoundToInt(baseCost * Mathf.Pow(costEvolutionFactor, level)));

    public float GetSpeedFactor(int level)
    {
        return Mathf.Pow(speedMultiplier, level);
    }
}