using UnityEngine;

public class RatingModalVisual : MonoBehaviour
{
    [SerializeField] private GameObject ratingModal;

    public void ShowModal()
    {
        ratingModal.SetActive(true);
    }

    public void HideModal()
    {
        ratingModal.SetActive(false);
    }

    public void RefuseRating()
    {
        GameManager.Instance.DayCycleManager.RefuseRating();
        HideModal();
    }
}