using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GrillMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler
{
    [HideInInspector] public UnityEvent<GameObject> OpenGrill;
    [HideInInspector] public UnityEvent<GameObject> CloseGrill;

    private bool isDraggingTacos = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDraggingTacos = false;

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var hit in results)
        {
            if (hit.gameObject.GetComponent<TacosMovemement>() != null)
            {
                if (GameManager.Instance.GrillManager.isGrillOpened)
                {
                    isDraggingTacos = true;
                }
                break;
            }
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDraggingTacos)
        {
            eventData.pointerDrag = null;
            return;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggingTacos)
        {
            return;
        }
        float scrollValue = eventData.delta.y;
        if (scrollValue > 0)
        {
            OpenGrill.Invoke(gameObject);
        }
        else if (scrollValue < 0)
        {
            CloseGrill.Invoke(gameObject);
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (!dropped.TryGetComponent<TacosDisplayer>(out var tacosDisplayer)) { return; }
        var tacos = tacosDisplayer.tacosData;
        if (tacos == null) { return; }
        GameManager.Instance.GrillManager.AddTacosToGrill(tacos);
    }
}