using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite unprocessedSprite;
    public Sprite processedSprite;
    public Sprite wastedSprite;
    public float processingTime;
    public float wastingTimeOffset;
    public IngredientCategoryEnum category;
    public bool isUnlockedFromTheBeginning;
    public int priceToUnlock = 0;
    public int priceToRefill = 1;

    public bool NeedProcessing()
    {
        return category == IngredientCategoryEnum.MEAT;
    }

}

public enum IngredientCategoryEnum
{
    MEAT,
    VEGETABLE,
    FRIES,
}
