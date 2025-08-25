using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private SettingsVisuals settingsVisuals;

    void Awake()
    {
        settingsVisuals = FindFirstObjectByType<SettingsVisuals>(FindObjectsInactive.Include);
    }

    public void ShowSettingsModal()
    {
        GameManager.Instance.PauseGame();
        settingsVisuals.ShowModal();
    }

    public void HideSettingsModal()
    {
        GameManager.Instance.ResumeGame();
        settingsVisuals.HideModal();
    }
}