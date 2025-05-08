using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    private string _channelId = "clicker_channel";
    private string _studentName = "Matias Pulido";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeNotifications();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeNotifications()
    {
#if UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = _channelId,
            Name = "Game Notifications",
            Importance = Importance.Default,
            Description = "Notifications for the Clicker game"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
#endif
    }

    public void ScheduleReturnNotification()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        var notification = new AndroidNotification()
        {
            Title = "¡Vuelve a jugar!",
            Text = _studentName + " jugó por última vez hace 10 minutos",
            FireTime = System.DateTime.Now.AddMinutes(10),
            SmallIcon = "icon_small",
            LargeIcon = "icon_large"
        };

        AndroidNotificationCenter.SendNotification(notification, _channelId);
#endif
    }

    public void SetStudentName(string name)
    {
        _studentName = name;
    }
}