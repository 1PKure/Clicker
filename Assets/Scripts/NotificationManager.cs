using UnityEngine;
#if !UNITY_WEBGL
using Unity.Notifications.Android;
#endif
using UnityEngine.Android;
using System.Collections;


public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }
    private string _channelId = "clicker_channel";
    //private string _studentName = "Matias Pulido";

    void Start()
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
                Id = _channelId,
                Name = "Game",
                Importance = Importance.Default,
                Description = "Main notifications for the app.",
                Group = group.Id
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            StartCoroutine(RequestNotificationsPermission());

           
            PlayerPrefs.SetInt("NotisChanel_Created", 1);
            PlayerPrefs.Save();
        }
        else
        {
           
            ScheduleNotification();
        }
    }

    private IEnumerator RequestNotificationsPermission()
    {
        
        var request = new PermissionRequest();

        while (request.Status == PermissionStatus.RequestPending)
            yield return new WaitForEndOfFrame();

        
        ScheduleNotification();
    }

    private void ScheduleNotification()
    {
        
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        
        var notification10Minutes = new AndroidNotification()
        {
            Title = "TP01 Portabilida y optimización",
            Text = "Juego creado por Matias Pulido",
            FireTime = System.DateTime.Now.AddMinutes(10),
        };

        
        AndroidNotificationCenter.SendNotification(notification10Minutes, _channelId);
    }
}