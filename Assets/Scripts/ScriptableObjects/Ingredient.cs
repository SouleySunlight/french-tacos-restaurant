using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public new string name;
    public GameObject inTacosSprite;
    public GameObject unprocessedSprite;
    public GameObject processedSprite;
    public float processingTime;

    public IngredientCategoryEnum category;

}

public enum IngredientCategoryEnum
{
    MEAT,
    VEGETABLE
}
