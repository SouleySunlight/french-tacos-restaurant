using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite unprocessedSprite;
    public Sprite processedSprite;
    public Sprite wastedSprite;
    public Sprite inTacosSprite;
    public Sprite inTacosSpriteAlternative;
    public Sprite uncookedFryerSprite;
    public Sprite cookedFryerSprite;
    public Sprite burntFryerSprite;
    public float processingTime;
    public float wastingTimeOffset;
    public IngredientCategoryEnum category;
    public bool isUnlockedFromTheBeginning;
    public float priceToUnlock = 0;
    public float priceToRefill = 1;
    public bool canBeAddedToTacos = true;
    public bool canBePurshased = true;
    public bool inEveryTacos = false;
    public int popularity = 0;
    public ProcessingMethodEnum processingMethod = ProcessingMethodEnum.NONE;

    public bool NeedProcessing()
    {
        return category == IngredientCategoryEnum.MEAT || category == IngredientCategoryEnum.FRIES || category == IngredientCategoryEnum.SAUCE_GRUYERE;
    }

}

public enum IngredientCategoryEnum
{
    MEAT,
    VEGETABLE,
    FRIES,
    SAUCE_GRUYERE_INGREDIENT,
    SAUCE_GRUYERE,
    SAUCE
}

public enum ProcessingMethodEnum
{
    HOTPLATE,
    FRYER,
    NONE
}
