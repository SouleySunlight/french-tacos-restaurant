using UnityEngine;

public class TutoButtonsDisplayer : MonoBehaviour
{

    public void OnClick()
    {
        GameManager.Instance.TutoManager.ShowTutorial(PlayzoneVisual.currentView);
    }
    public void OnClickOnClose()
    {
        GameManager.Instance.TutoManager.HideTutorial();
    }
}