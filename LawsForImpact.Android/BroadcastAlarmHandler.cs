using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LawsForImpact.Models;
using AndroidApp = Android.App.Application;

namespace LawsForImpact.Droid
{

    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class BroadcastAlarmHandler : BroadcastReceiver
    {
        NotificationManager manager;
        bool channelInitialized = false;
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        public const string LocalNotificationKey = "LocalNotification";

        public override void OnReceive(Context context, Intent intent)
        {
 

            AndroidNotificationManager makeNewNotification = new AndroidNotificationManager();
            makeNewNotification.ScheduleNotification("second notification", "success great job");

            //var alarmManager = GetAlarmManager();
            //alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, 2000, pendingIntent);
        }

        //private AlarmManager GetAlarmManager()
        //{

        //    var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
        //    return alarmManager;
        //}


        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
    }
}