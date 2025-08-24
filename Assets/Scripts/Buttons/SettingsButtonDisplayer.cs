using UnityEngine;

public class SettingsButtonDisplayer : MonoBehaviour
{

    public void OnClick()
    {
        GameManager.Instance.SettingsManager.ShowSettingsModal();
    }

    public void OnCloseButtonClicked()
    {
        GameManager.Instance.SettingsManager.HideSettingsModal();

    }
}