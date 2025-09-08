using UnityEngine;
using System;
using UnityEngine.Localization.Settings;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class NotificationManager : MonoBehaviour
{
    private const string androidChannelId = "tacos_channel";

    public void ScheduleNotification()
    {
        CancelAllNotifications();
        var locale = LocalizationSettings.SelectedLocale.Identifier.Code;
        string title = locale == "fr" ? "Tes clients ont faim 🌮" : "Your customers are hungry 🌮";
        string subtitle = locale == "fr" ? "Les tacos ne se font pas tout seul !" : "Tacos don't make themselves!";

#if UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = androidChannelId,
            Name = "French Tacos Restaurant reminder",
            Importance = Importance.Default,
            Description = "Notification to remind the player to play",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification
        {
            Title = title,
            Text = subtitle,
            FireTime = DateTime.Now.AddHours(24)
        };

        AndroidNotificationCenter.SendNotification(notification, androidChannelId);
#endif

#if UNITY_IOS
        var authOptions = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
        iOSNotificationCenter.RequestAuthorization(authOptions, granted =>
        {
            Debug.Log("Permission notifications iOS : " + granted);
        });

        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(24, 0, 0),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            Identifier = "tacos_notif",
            Title = title,
            Body = subtitle,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }

    public void CancelAllNotifications()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
#endif
#if UNITY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif
    }

    public void RequestPermission()
    {
#if UNITY_IOS
        var authOptions = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
        iOSNotificationCenter.RequestAuthorization(authOptions, granted =>
        {
            Debug.Log("Permission notifications iOS : " + granted);
        });
#endif

#if UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            UnityEngine.Android.Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
#endif
    }
}
