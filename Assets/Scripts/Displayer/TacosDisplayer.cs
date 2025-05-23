using UnityEngine;
using UnityEngine.UI;

public class TacosDisplayer : MonoBehaviour
{
    public Tacos tacosData;
    [SerializeField] private Sprite ungrilledTacosImage;
    [SerializeField] private Sprite grilledTacosImage;
    [SerializeField] private Sprite burntTacos;



    void Start()
    {
        UpdateTacosVisual();
    }
    public void UpdateTacosVisual()
    {
        if (tacosData.IsBurnt())
        {
            GetComponentInChildren<Image>().sprite = burntTacos;
            return;
        }

        GetComponentInChildren<Image>().sprite = tacosData.IsGrilled() ? grilledTacosImage : ungrilledTacosImage;
    }

}
