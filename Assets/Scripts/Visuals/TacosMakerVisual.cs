using UnityEngine;

public class TacosMakerVisual : MonoBehaviour
{
    [SerializeField] private GameObject tacosMakerWindow;
    [SerializeField] private GameObject tortillaPrefab;
    [SerializeField] private RectTransform onCreationTacosTransform;
    private GameObject onCreationTacos;

    public void OpenTacosMakerWindow()
    {
        tacosMakerWindow.SetActive(true);
    }

    public void CloseTacosMakerWindow()
    {
        tacosMakerWindow.SetActive(false);
    }

    public void CreateTacos()
    {
        onCreationTacos = Instantiate(tortillaPrefab, onCreationTacosTransform.position, Quaternion.identity, onCreationTacosTransform);
    }
}
