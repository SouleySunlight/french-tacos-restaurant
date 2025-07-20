using UnityEngine;
using UnityEngine.UI;

public class SidebarButtonDisplayer : MonoBehaviour
{
    public SidebarOptions sidebarOption;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image borderImage;

    void Start()
    {
        iconImage.sprite = sidebarOption.icon;
        borderImage.color = Colors.GetColorFromHexa(sidebarOption.color);
    }

}