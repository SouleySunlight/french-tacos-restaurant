using UnityEngine;
using UnityEngine.UI;

public class TacosDisplayer : MonoBehaviour
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

}
