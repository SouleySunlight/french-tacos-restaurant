using System.Collections.Generic;
using UnityEngine;

public static class UIPlacement
{
    private const int NUMBER_OF_BUTTON_PER_ROW = 4;
    private const int NUMBER_OF_INDICATOR_PER_ROW = 3;

    public static void PlaceIngredientButtons(List<GameObject> buttons, float totalWidth)
    {

        const float buttonWidth = 120f;
        const float buttonHeight = 120f;

        var totalButtons = buttons.Count;

        for (int i = 0; i < totalButtons; i++)
        {
            var button = buttons[i].GetComponent<RectTransform>();

            button.anchorMin = new Vector2(0, 0.6f);
            button.anchorMax = new Vector2(0, 0.6f);
            button.pivot = new Vector2(0.5f, 0f);

            int col = i % NUMBER_OF_BUTTON_PER_ROW;
            int row = i / NUMBER_OF_BUTTON_PER_ROW;

            int itemsInRow = Mathf.Min(totalButtons - row * NUMBER_OF_BUTTON_PER_ROW, NUMBER_OF_BUTTON_PER_ROW);

            float totalRowWidth = itemsInRow * buttonWidth;

            float spaceLeft = totalWidth - totalRowWidth;

            float gap = spaceLeft / (itemsInRow + 1);

            float x = gap * (col + 1) + buttonWidth * col + 100;

            float y = -(buttonHeight + 20f) * row;

            button.anchoredPosition = new Vector2(x, y);
        }
    }

    public static void PlaceIngredientIndicators(List<GameObject> indicators, float totalWidth)
    {
        const float indicatorWidth = 230f;
        const float indicatorHeight = 110f;

        var totalIndicators = indicators.Count;

        for (int i = 0; i < totalIndicators; i++)
        {
            var indicator = indicators[i].GetComponent<RectTransform>();

            indicator.anchorMin = new Vector2(0, 0.8f);
            indicator.anchorMax = new Vector2(0, 0.8f);
            indicator.pivot = new Vector2(0.5f, 0f);

            int col = i % NUMBER_OF_INDICATOR_PER_ROW;
            int row = i / NUMBER_OF_INDICATOR_PER_ROW;

            int itemsInRow = Mathf.Min(totalIndicators - row * NUMBER_OF_INDICATOR_PER_ROW, NUMBER_OF_INDICATOR_PER_ROW);

            float totalRowWidth = itemsInRow * indicatorWidth;

            float spaceLeft = totalWidth - totalRowWidth;

            float gap = spaceLeft / (itemsInRow + 1);

            float x = gap * (col + 1) + indicatorWidth * col + 175;

            float y = -(indicatorHeight + 20f) * row - 30;

            indicator.anchoredPosition = new Vector2(x, y);
        }
    }
}