using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationsMannager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Canal_Principal",
            Importance = Importance.High,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(c);

        var notification = new AndroidNotification();
        notification.Title = "Atenção!";
        notification.Text = "O Vitu te ama";
        notification.FireTime = System.DateTime.Now.AddHours(4);

        var notification2 = new AndroidNotification();
        notification.Title = "Sem nevosa!";
        notification.Text = "O Vitu virtual tá aqui";
        notification.FireTime = System.DateTime.Now.AddHours(6);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
        AndroidNotificationCenter.SendNotification(notification2, "channel_id");
    }
}
