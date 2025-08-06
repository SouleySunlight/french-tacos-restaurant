using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GrillMovement : MonoBehaviour, IDragHandler
{
    [HideInInspector] public UnityEvent<GameObject> OpenGrill;
    [HideInInspector] public UnityEvent<GameObject> CloseGrill;

    public void OnDrag(PointerEventData eventData)
    {
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

}