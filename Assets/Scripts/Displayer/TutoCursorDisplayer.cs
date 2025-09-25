using System.Collections;
using UnityEngine;

public class TutoCursorDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject cursorObject;

    private bool shouldStopMoving = false;

    public void MoveCursor(Vector3 startingPos, Vector3 targetPos, float duration, float angle = 45)
    {
        shouldStopMoving = false;
        cursorObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        ShowCursor();
        StartCoroutine(MoveCursorCoroutine(startingPos, targetPos, duration));
    }

    IEnumerator MoveCursorCoroutine(Vector3 startingPos, Vector3 targetPos, float duration)
    {
        while (!shouldStopMoving)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                cursorObject.transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            cursorObject.transform.position = targetPos;
            yield return new WaitForSeconds(0.5f);
            elapsedTime = 0f;
        }
    }

    public void ShowCursor()
    {
        cursorObject.SetActive(true);
    }

    public void HideCursor()
    {
        cursorObject.SetActive(false);
    }

    public void StopCursorMovement()
    {
        shouldStopMoving = true;
        HideCursor();
    }
}