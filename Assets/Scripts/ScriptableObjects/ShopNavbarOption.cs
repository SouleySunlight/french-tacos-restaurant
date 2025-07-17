using UnityEngine;

[CreateAssetMenu(fileName = "ShopNavbarOption", menuName = "ShopNavbarOption", order = 0)]
public class ShopNavbarOption : ScriptableObject
{
    public ShopViewEnum shopViewEnum;
    public Sprite icon;
}