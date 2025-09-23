using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class CircleMaskRaycast : MonoBehaviour, ICanvasRaycastFilter
{
    // centre / radius en coordonnées normalisées [0..1] (set via TutorialOverlay)
    private Vector2 center = new Vector2(0.5f, 0.5f);
    private float radius = 0.18f;

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetCircle(Vector2 normalizedCenter, float normalizedRadius)
    {
        center = normalizedCenter;
        radius = normalizedRadius;
    }

    // Called by the GraphicRaycaster: return false to let the event pass through.
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, eventCamera, out localPoint))
            return true; // si échec, ne pas bloquer

        Vector2 normalized = new Vector2(
            (localPoint.x / rect.rect.width) + 0.5f,
            (localPoint.y / rect.rect.height) + 0.5f
        );

        // appliquer la même correction d'aspect que le shader (width / height)
        float aspect = rect.rect.width / rect.rect.height;
        Vector2 delta = normalized - center;
        delta.x *= aspect;
        float d = delta.magnitude;

        // si inside hole -> return false (pas valide) -> laisse passer le clic.
        return !(d < radius);
    }
}
