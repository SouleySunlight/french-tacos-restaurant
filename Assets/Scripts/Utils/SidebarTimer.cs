using System.Collections.Generic;

public static class SidebarTimer
{
    public static void UpdateSidebarTimer(List<float> processingTime, List<float> totalProcessingTime, ViewToShowEnum viewToShowEnum)
    {
        float maxTimer = 0;
        for (int i = 0; i < processingTime.Count; i++)
        {
            if (processingTime[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }
            var percentage = processingTime[i] / totalProcessingTime[i];
            if (percentage > maxTimer)
            {
                maxTimer = percentage;
            }
        }
        GameManager.Instance.SidebarManager.UpdateButtonTimer(viewToShowEnum, maxTimer);
    }
}