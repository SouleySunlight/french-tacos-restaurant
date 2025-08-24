using UnityEngine;
using UnityEngine.UI;

public class BackgroundVisuals : MonoBehaviour
{

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite tacosMakerBackground;
    [SerializeField] private Sprite grillBackground;
    [SerializeField] private Sprite hotplateBackground;
    [SerializeField] private Sprite fryerBackground;
    [SerializeField] private Sprite sauceGruyereBackground;
    [SerializeField] private Sprite checkoutBackground;





    public void UpdateVisuals()
    {
        backgroundImage.sprite = PlayzoneVisual.currentView switch
        {
            ViewToShowEnum.TACOS_MAKER => tacosMakerBackground,
            ViewToShowEnum.GRILL => grillBackground,
            ViewToShowEnum.HOTPLATE => hotplateBackground,
            ViewToShowEnum.FRYER => fryerBackground,
            ViewToShowEnum.SAUCE_GRUYERE => sauceGruyereBackground,
            ViewToShowEnum.CHECKOUT => checkoutBackground,
            _ => tacosMakerBackground,
        };
    }

}