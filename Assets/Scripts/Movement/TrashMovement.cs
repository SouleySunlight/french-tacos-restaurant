using UnityEngine;
using UnityEngine.EventSystems;

public class TrashMovement : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData pointerEventData)
    {
        GameObject dropped = pointerEventData.pointerDrag;
        Debug.Log(dropped.GetComponent<TacosDisplayer>().tacosData.ingredients[0]);
    }
}