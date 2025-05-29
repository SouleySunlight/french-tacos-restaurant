using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public new string name;
    public GameObject sprite;
    public IngredientCategoryEnum category;

}

public enum IngredientCategoryEnum
{
    MEAT,
    VEGETABLE
}
