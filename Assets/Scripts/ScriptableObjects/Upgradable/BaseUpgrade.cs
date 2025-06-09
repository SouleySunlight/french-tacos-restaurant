using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Upgradable/Base")]
public abstract class BaseUpgrade : ScriptableObject
{
    public string id;
    public int baseCost = GlobalConstant.DEFAULT_UPGRADE_BASE_COST;
    public float costEvolutionFactor = GlobalConstant.DEFAULT_UPGRADE_COST_FACTOR;
    public int maxLevel = GlobalConstant.DEFAULT_UPGRADE_MAX_LEVEL;

    public int GetCostAtLevel(int level)
        => Mathf.RoundToInt(baseCost * Mathf.Pow(costEvolutionFactor, level));

    public abstract float GetEffect(int level);
}