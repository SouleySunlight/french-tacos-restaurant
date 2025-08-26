using UnityEngine;
using UnityEngine.EventSystems;

public class TrashMovement : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData pointerEventData)
    {
        GameObject dropped = pointerEventData.pointerDrag;
        if (!dropped.TryGetComponent<TacosDisplayer>(out var tacosDisplayer)) { return; }
        if (PlayzoneVisual.currentView == ViewToShowEnum.GRILL)
        {
            GameManager.Instance.GrillManager.DiscardTacos(tacosDisplayer.tacosData);
        }
    }
}