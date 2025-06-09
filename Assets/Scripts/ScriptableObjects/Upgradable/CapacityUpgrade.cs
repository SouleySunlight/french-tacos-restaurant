using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Upgradable/Capacity")]
public class CapacityUpgrade : BaseUpgrade
{
    public float capacityTerm = GlobalConstant.DEFAULT_CAPACITY_UPGRADE;

    public override float GetEffect(int level)
    {
        return level * capacityTerm;
    }
}