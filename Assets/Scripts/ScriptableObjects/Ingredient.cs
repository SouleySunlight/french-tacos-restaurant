using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public new string name;
    public Sprite inTacosSprite;
    public Sprite unprocessedSprite;
    public Sprite processedSprite;
    public float processingTime;

    public IngredientCategoryEnum category;

}

public enum IngredientCategoryEnum
{
    MEAT,
    VEGETABLE
}
