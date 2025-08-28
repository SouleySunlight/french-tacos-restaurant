using UnityEngine;
using UnityEngine.UI;

public class SidebarButtonDisplayer : MonoBehaviour
{
    public SidebarOptions sidebarOption;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private GameObject roundedCompletionbar;

    void Start()
    {
        iconImage.sprite = sidebarOption.icon;
        borderImage.color = Colors.GetColorFromHexa(sidebarOption.color);
    }

    public void UpdateTimer(float percentage)
    {
        roundedCompletionbar.GetComponent<SidebarCompletionBar>().UpdateTimer(percentage);
    }

}