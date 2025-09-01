using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "Reward", order = 0)]
public class Reward : ScriptableObject
{
    public int numberOfTacosToUnlock;
    public RewardType rewardType;
    public Ingredient ingredientToUnlock;
}

public enum RewardType
{
    UNLOCK_INGREDIENT,
    PRICE_INCREASE,
    MORE_ORDERS,
    INCREASE_MAX_INGREDIENTS
}