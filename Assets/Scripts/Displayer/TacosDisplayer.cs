using UnityEngine;
using UnityEngine.UI;

public class TacosDisplayer : MonoBehaviour
{
    public Tacos tacosData;
    [SerializeField] private Sprite ungrilledTacosImage;
    [SerializeField] private Sprite grilledTacosImage;
    [SerializeField] private Sprite burntTacos;
    [SerializeField] private Image tacosImage;



    void Start()
    {
        UpdateTacosVisual();
    }
    public void UpdateTacosVisual()
    {
        if (tacosData.IsBurnt())
        {
            tacosImage.sprite = burntTacos;
            return;
        }

        tacosImage.sprite = tacosData.IsGrilled() ? grilledTacosImage : ungrilledTacosImage;
    }

}
