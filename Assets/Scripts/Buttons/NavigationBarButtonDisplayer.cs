using UnityEngine;
using UnityEngine.UI;

public class NavigationBarButtonDisplayer : MonoBehaviour
{
    public ShopNavbarOption shopNavbarOption;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image selectedBottomBar;


    void Start()
    {
        iconImage.sprite = shopNavbarOption.icon;

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (ShopViewVisual.currentView == shopNavbarOption.shopViewEnum)
        {
            SetSelectedVisual();
        }
        else
        {
            SetIdleVisual();
        }

    }

    void SetIdleVisual()
    {
        backgroundImage.color = Colors.GetColorFromHexa(Colors.IDLE_NAVBAR_BUTTON_BACKGROUND);
        iconImage.color = Colors.GetColorFromHexa(Colors.IDLE_NAVBAR_BUTTON_ICON);
        selectedBottomBar.gameObject.SetActive(false);
    }

    void SetSelectedVisual()
    {
        backgroundImage.color = Colors.GetColorFromHexa(Colors.SELECTED_NAVBAR_BUTTON_BACKGROUND);
        iconImage.color = Colors.GetColorFromHexa(Colors.SELECTED_NAVBAR_BUTTON_ICON);
        selectedBottomBar.gameObject.SetActive(true);
    }
}