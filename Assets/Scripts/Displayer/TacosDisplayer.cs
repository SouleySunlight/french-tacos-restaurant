using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TacosDisplayer : MonoBehaviour, IPointerDownHandler
{
    public Tacos tacosData;
    [SerializeField] private Sprite ungrilledTacosImage;
    [SerializeField] private Sprite grilledTacosImage;


    void Start()
    {
        UpdateTacosVisual();
    }
    public void UpdateTacosVisual()
    {
        GetComponentInChildren<Image>().sprite = tacosData.isGrilled ? grilledTacosImage : ungrilledTacosImage;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(tacosData.guid);
    }
}
