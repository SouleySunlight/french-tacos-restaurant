using UnityEngine;

public class TutoButtonsDisplayer : MonoBehaviour
{

    public void OnClick()
    {
        GameManager.Instance.HelpWindowManager.ShowTutorial(PlayzoneVisual.currentView);
    }
    public void OnClickOnClose()
    {
        GameManager.Instance.HelpWindowManager.HideTutorial();
    }
}