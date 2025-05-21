using UnityEngine;

public class TacosMakerVisual : MonoBehaviour
{
    [SerializeField] private GameObject tacosMakerWindow;

    public void OpenTacosMakerWindow()
    {
        tacosMakerWindow.SetActive(true);
    }

    public void CloseTacosMakerWindow()
    {
        tacosMakerWindow.SetActive(false);
    }
}
