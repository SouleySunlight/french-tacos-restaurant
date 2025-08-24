using UnityEngine;

public class SettingsVisuals : MonoBehaviour
{
    [SerializeField] GameObject settingsModal;

    public void ShowModal()
    {
        settingsModal.SetActive(true);

    }
    public void HideModal()
    {
        settingsModal.SetActive(false);

    }
}