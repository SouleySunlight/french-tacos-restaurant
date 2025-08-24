using UnityEngine;

public class ToNextDayButtonBehavior : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.DayCycleManager.ToNextDay();
    }
}