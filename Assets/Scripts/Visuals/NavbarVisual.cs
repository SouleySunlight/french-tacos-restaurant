using System.Collections.Generic;
using UnityEngine;

public class NavbarVisual : MonoBehaviour
{

    [SerializeField] private GameObject navbarButton;
    [SerializeField] private List<ShopNavbarOption> options;

    void Awake()
    {
        var index = 0;
        float totalWidth = GetComponent<RectTransform>().rect.width;
        var itemWidth = totalWidth / options.Count;
        foreach (var option in options)
        {
            var createdOption = Instantiate(navbarButton, this.transform);
            var rectTransform = createdOption.GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0f, 0.5f);
            rectTransform.anchorMax = new Vector2(0f, 0.5f);
            rectTransform.pivot = new Vector2(0f, 0.5f);

            rectTransform.sizeDelta = new Vector2(itemWidth, rectTransform.sizeDelta.y);
            rectTransform.anchoredPosition = new Vector2(index * itemWidth, 0f);

            createdOption.GetComponent<NavigationBarButtonDisplayer>().shopNavbarOption = options[index];
            index++;
        }
    }


}