using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private BackgroundVisuals backgroundVisuals;

    void Awake()
    {
        backgroundVisuals = FindFirstObjectByType<BackgroundVisuals>();
    }

    public void UpdateBackground()
    {
        backgroundVisuals.UpdateVisuals();
    }
}