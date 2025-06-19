using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IngredientButtonDisplayer : MonoBehaviour
{

    public Ingredient ingredientData;

    [SerializeField] private Button button;
    [SerializeField] private Image ingredientImage;


    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        ingredientImage.sprite = ingredientData.processedSprite;
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}