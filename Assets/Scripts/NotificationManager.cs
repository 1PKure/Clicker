
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;


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
    void Start()
    {
        if (AndroidVersion >= 33 && !Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }
    int AndroidVersion
    {
        get
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return version.GetStatic<int>("SDK_INT");
            }
        }
    }

    internal void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
    }

    internal void PermissionCallbacks_PermissionGranted(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
    }

    internal void PermissionCallbacks_PermissionDenied(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
    }
    private void InitializeNotifications()
    {

        var channel = new AndroidNotificationChannel()
        {
            Id = _channelId,
            Name = "Game Notifications",
            Importance = Importance.Default,
            Description = "Notifications for the Clicker game"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

    }

    public void ScheduleReturnNotification()
    {

        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        var notification = new AndroidNotification()
        {
            Title = "¡Vuelve a jugar!",
            Text = _studentName + " jugó por última vez hace 10 minutos",
            FireTime = System.DateTime.Now.AddMinutes(10),
        };

        AndroidNotificationCenter.SendNotification(notification, _channelId);

    }

    public void SetStudentName(string name)
    {
        _studentName = name;
    }
}