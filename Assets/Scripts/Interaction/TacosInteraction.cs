using UnityEngine;
using UnityEngine.EventSystems;

public class TacosInteraction : MonoBehaviour, IPointerDownHandler
{
    public Tacos tacosData;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(tacosData.guid);
    }
}
