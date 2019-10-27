using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    public string channelID;
    public int identifier;
    public List<string> titles;
    public List<string> texts;
    void Start()
    {
        
        CreateChannel();
        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        
        if (notificationIntentData!=null)
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }
       
          
        
    }
    private void OnApplicationFocus(bool hasFocus)
    {

        if(hasFocus)
        {
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
            if (notificationIntentData != null)
            {
                AndroidNotificationCenter.CancelAllNotifications();
                Start();
            }
            AndroidNotificationCenter.CancelAllScheduledNotifications();

        }
        else
        {
            int randomText = Random.Range(0, texts.Count);
              var notification = GenerateNotification
               (
               titles[randomText], texts[randomText],
               System.DateTime.Now.AddHours(12), true
               );

                SendNotification(notification);
            
        }

    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void CreateChannel()
    {

        var c = new AndroidNotificationChannel()
        {
            Id = channelID,
            Name = "Updates",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);


    }
    public void SendNotification(AndroidNotification notification)
    {
        identifier = AndroidNotificationCenter.SendNotification(notification, channelID);
    }
    public AndroidNotification GenerateNotification(string title, string text, System.DateTime fireTime, bool isBig=false)
    {
        var notification = new AndroidNotification
        {
            Title = title,
            Text = text,
            SmallIcon = "small",
            LargeIcon = "large",
            FireTime = fireTime,
            
            
        };
        if (isBig)
            notification.Style = NotificationStyle.BigTextStyle;
        else notification.Style = NotificationStyle.None;
        return notification;
    }
   
}
