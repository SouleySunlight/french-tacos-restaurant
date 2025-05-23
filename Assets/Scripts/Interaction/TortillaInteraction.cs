using UnityEngine;
using UnityEngine.EventSystems;

public class TortillaInteraction : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        FindFirstObjectByType<GameManager>().WrapTacos();
    }
}
