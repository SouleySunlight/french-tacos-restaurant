using UnityEngine;
using UnityEngine.EventSystems;

public class TrashMovement : MonoBehaviour, IDropHandler
{

    [SerializeField] private AudioClip trashShound;

    public void OnDrop(PointerEventData pointerEventData)
    {
        GameObject dropped = pointerEventData.pointerDrag;
        if (!dropped.TryGetComponent<TacosDisplayer>(out var tacosDisplayer)) { return; }
        if (PlayzoneVisual.currentView == ViewToShowEnum.GRILL)
        {
            GameManager.Instance.SoundManager.PlaySFX(trashShound);
            GameManager.Instance.GrillManager.DiscardTacos(tacosDisplayer.tacosData);
        }
    }
}