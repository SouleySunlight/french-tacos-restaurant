using UnityEngine;

public class OpenTacosMaker : MonoBehaviour
{
    public void OnClick()
    {
        FindFirstObjectByType<TacosMakerVisual>(FindObjectsInactive.Include).OpenTacosMakerWindow();
    }
}
