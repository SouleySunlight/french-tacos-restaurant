using UnityEngine;

public class ShopButtonBehavior : MonoBehaviour
{
    public void OnClick()
    {
        FindFirstObjectByType<WindowVisual>().DisplayWindow(WindowsEnum.SHOP);
    }
}