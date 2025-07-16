using UnityEngine;

public class KitchenButtonBehavior : MonoBehaviour
{
    public void OnClick()
    {
        FindFirstObjectByType<WindowVisual>().DisplayWindow(WindowsEnum.GAME);
    }
}