using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavbarVisual : MonoBehaviour
{

    [SerializeField] private GameObject navbarButtonPrefab;
    [SerializeField] private List<ShopNavbarOption> options;

    private List<GameObject> navbarButtons = new();

    void Awake()
    {
        var index = 0;
        float totalWidth = GetComponent<RectTransform>().rect.width;
        var itemWidth = totalWidth / options.Count;
        foreach (var option in options)
        {
            var createdOption = Instantiate(navbarButtonPrefab, this.transform);
            var rectTransform = createdOption.GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0f, 0.5f);
            rectTransform.anchorMax = new Vector2(0f, 0.5f);
            rectTransform.pivot = new Vector2(0f, 0.5f);

            rectTransform.sizeDelta = new Vector2(itemWidth, rectTransform.sizeDelta.y);
            rectTransform.anchoredPosition = new Vector2(index * itemWidth, 0f);

            createdOption.GetComponent<NavigationBarButtonDisplayer>().shopNavbarOption = options[index];
            createdOption.GetComponent<Button>().onClick.AddListener(() => UpdateView(option.shopViewEnum));

            navbarButtons.Add(createdOption);

            index++;
        }
    }

    void Start()
    {
        UpdateView(ShopViewEnum.WORKERS);
    }

    void UpdateView(ShopViewEnum viewToShow)
    {
        FindFirstObjectByType<ShopViewVisual>().DisplayView(viewToShow);
        UpdateButtonsVisual();
    }

    void UpdateButtonsVisual()
    {
        foreach (GameObject button in navbarButtons)
        {
            button.GetComponent<NavigationBarButtonDisplayer>().UpdateVisual();
        }
    }


}