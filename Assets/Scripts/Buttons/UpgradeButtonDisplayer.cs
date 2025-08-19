using TMPro;
using UnityEngine;

public class UpgradeButtonDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject upgradeButton;

    public void UpdateVisual()
    {
        var currentView = PlayzoneVisual.currentView;

        if (currentView == ViewToShowEnum.TACOS_MAKER || currentView == ViewToShowEnum.CHECKOUT)
        {
            upgradeButton.SetActive(false);
            return;
        }
        upgradeButton.SetActive(true);
    }


}