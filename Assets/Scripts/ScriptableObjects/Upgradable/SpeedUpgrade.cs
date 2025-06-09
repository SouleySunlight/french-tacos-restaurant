using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Upgradable/Speed")]
public class SpeedUpgradeSO : BaseUpgrade
{
    public float speedMultiplier = GlobalConstant.DEFAULT_SPEED_UPGRADE;

    public override float GetEffect(int level)
    {
        return Mathf.Pow(speedMultiplier, level);
    }
}