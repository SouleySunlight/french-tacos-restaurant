using UnityEngine;

public static class Colors
{
    public static readonly string GREEN_VEGETABLE = "#27861E";
    public static readonly string BROWN_MEAT = "#84694C";
    public static readonly string YELLOW_SAUCE = "#DDCF0F";
    public static readonly string GREY_EVERY_TACOS = "#B6B6B6";

    public static Color GetColorFromHexa(string code)
    {
        if (ColorUtility.TryParseHtmlString(code, out Color color))
        {
            return color;
        }
        return Color.white;
    }
}