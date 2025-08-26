using UnityEngine;
using UnityEngine.EventSystems;

public class TrashMovement : MonoBehaviour
{

    [SerializeField] private AudioClip trashShound;


    public void PlayTrashSound()
    {
        GameManager.Instance.SoundManager.PlaySFX(trashShound);
    }
}