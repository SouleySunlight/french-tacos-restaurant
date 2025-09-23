using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TutorialPanelVisual : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Canvas rootCanvas;           // le Canvas racine
    [SerializeField] private Image overlayImage;         // image avec le material UI/CircleMask
    [SerializeField] private CircleMaskRaycast raycastFilter; // script sur la même image (voir plus bas)

    private Material runtimeMat;

    void Awake()
    {
        if (overlayImage == null) overlayImage = GetComponent<Image>();
        // clone du material pour ne pas modifier le sharedMaterial
        if (overlayImage.material != null)
            runtimeMat = new Material(overlayImage.material);
        else
            runtimeMat = new Material(Shader.Find("UI/CircleMask"));
        overlayImage.material = runtimeMat;

        // s'assurer que l'overlay recouvre tout le canvas (anchors stretch) et que Raycast Target = true
        overlayImage.raycastTarget = true;
    }

    // public API
    public void FocusOn(RectTransform target, float radius = 0.18f)
    {
        panel.SetActive(true);
        StartCoroutine(FocusCoroutine(target, radius));
    }

    private IEnumerator FocusCoroutine(RectTransform target, float radius)
    {
        // attendre 1 frame / forcer le layout pour que la position de l'UI soit correcte
        yield return null;
        Canvas.ForceUpdateCanvases();
        yield return null;

        // choisir la caméra correcte selon le renderMode du Canvas
        Camera cam = null;
        if (rootCanvas != null && (rootCanvas.renderMode == RenderMode.ScreenSpaceCamera || rootCanvas.renderMode == RenderMode.WorldSpace))
            cam = rootCanvas.worldCamera;

        Vector3 worldCenter = target.position
            + target.rotation * Vector3.Scale(target.rect.size, (Vector2.one * 0.5f - target.pivot));

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldCenter);

        // convertir par rapport à l'Image overlay (les UV du shader sont relatifs à l'image)
        RectTransform overlayRect = overlayImage.rectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(overlayRect, screenPos, cam, out localPoint);

        Vector2 normalized = new Vector2(
            (localPoint.x / overlayRect.rect.width) + 0.5f,
            (localPoint.y / overlayRect.rect.height) + 0.5f
        );

        // envoyer au shader (centre en UV 0..1)
        runtimeMat.SetVector("_Center", new Vector4(normalized.x, normalized.y, 0, 0));
        runtimeMat.SetFloat("_Radius", radius);

        // mettre à jour aussi le filtre de raycast pour laisser passer le clic au bon endroit
        if (raycastFilter != null)
            raycastFilter.SetCircle(normalized, radius);
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }
}
