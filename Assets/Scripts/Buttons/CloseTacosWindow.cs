using UnityEngine;

public class CloseTacosWindow : MonoBehaviour
{
    public void OnClick()
    {
        FindFirstObjectByType<TacosMakerVisual>(FindObjectsInactive.Include).CloseTacosMakerWindow();
    }
}
