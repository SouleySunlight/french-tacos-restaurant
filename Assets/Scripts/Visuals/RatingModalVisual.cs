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
        GameManager.Instance.SaveSettings();
    }

    public void RefuseRating()
    {
        GameManager.Instance.DayCycleManager.RefuseRating();
        HideModal();
    }

    public void AcceptRating()
    {
        GameManager.Instance.DayCycleManager.AcceptRating();
        HideModal();
    }
}