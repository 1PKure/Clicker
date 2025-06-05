using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }
    private string _androidChannelId = "clicker_channel";

    void Start()
    {
#if UNITY_ANDROID
        SetupAndroidChannel();
        StartCoroutine(RequestNotificationsPermission());
#elif UNITY_IOS
        StartCoroutine(RequestNotificationsPermission_iOS());
#endif
    }

#if UNITY_ANDROID
    private void SetupAndroidChannel()
    {
        if (!PlayerPrefs.HasKey("NotisChanel_Created"))
        {
            var group = new AndroidNotificationChannelGroup()
            {
                Id = "Main",
                Name = "Main Notifications"
            };
            AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

            var channel = new AndroidNotificationChannel()
            {
                Id = _androidChannelId,
                Name = "Game",
                Importance = Importance.Default,
                Description = "Main notifications for the app.",
                Group = group.Id
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            PlayerPrefs.SetInt("NotisChanel_Created", 1);
            PlayerPrefs.Save();
        }

        ScheduleNotification_Android();
    }

    private IEnumerator RequestNotificationsPermission()
    {

        var request = new PermissionRequest();

        while (request.Status == PermissionStatus.RequestPending)
            yield return new WaitForEndOfFrame();


        ScheduleNotification_Android();
    }

    private void ScheduleNotification_Android()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        var notification10Minutes = new AndroidNotification()
        {
            Title = "TP01 - Portabilidad y optimización",
            Text = "Matias Pulido, volve a jugar!",
            FireTime = System.DateTime.Now.AddMinutes(10),
        };

        AndroidNotificationCenter.SendNotification(notification10Minutes, _androidChannelId);
    }
#endif

#if UNITY_IOS
    private IEnumerator RequestNotificationsPermission_iOS()
    {
        using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound, true))
        {
            while (!req.IsFinished)
                yield return null;

            Debug.Log("iOS Notification Permission: " + req.Granted);
        }

        ScheduleNotification_iOS();
    }

    private void ScheduleNotification_iOS()
    {
        iOSNotificationCenter.RemoveAllScheduledNotifications();

        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new System.TimeSpan(0, 10, 0), // 10 minutes
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            Identifier = "_notification_01",
            Title = "TP01 - Portabilidad y optimización",
            Body = "Matias Pulido, volve a jugar!",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }
#endif
}
