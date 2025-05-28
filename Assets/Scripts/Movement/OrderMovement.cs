using UnityEngine;
using UnityEngine.EventSystems;

public class OrderMovement : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("coucou");
    }
}