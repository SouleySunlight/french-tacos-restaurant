using UnityEngine;

public class SidebarManager : MonoBehaviour
{
    private SidebarVisuals sidebarVisuals;

    void Awake()
    {
        sidebarVisuals = FindFirstObjectByType<SidebarVisuals>(FindObjectsInactive.Include);
    }

    public void UpdateButtonTimer(ViewToShowEnum viewToShow, float timer)
    {
        sidebarVisuals.UpdateTimer(viewToShow, timer);
    }

    public void UpdateSidebarButtons()
    {
        sidebarVisuals.UpdateSidebarVisual();
    }
}