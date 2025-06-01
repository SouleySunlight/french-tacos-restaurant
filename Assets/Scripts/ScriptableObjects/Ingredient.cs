using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public new string name;
    public Sprite unprocessedSprite;
    public Sprite processedSprite;
    public Sprite wastedSprite;
    public float processingTime;
    public float wastingTime;

    public IngredientCategoryEnum category;

}

public enum IngredientCategoryEnum
{
    MEAT,
    VEGETABLE
}
