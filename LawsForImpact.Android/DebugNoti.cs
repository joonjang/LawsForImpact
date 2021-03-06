﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using LawsForImpact.Droid;
using LawsForImpact.Models;
using LawsForImpact.Services;
using LawsForImpact.ViewModels;
using SQLite;
using AndroidApp = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(DebugNoti))]
namespace LawsForImpact.Droid
{
    class DebugNoti : IDebugNotiServ
    {
        int _notificationIconId { get; set; }

        internal string _randomNumber;

        public void LocalNotification(string title, string body, int id, DateTime notifyTime)
        {
          
            var intent = CreateIntent(id);
            var localNotification = new LocalNotification();
            localNotification.Title = title;
            localNotification.Body = body;
            localNotification.Index = id;
            localNotification.NotifyTime = notifyTime;

            if (_notificationIconId != 0)
            {
                localNotification.IconId = _notificationIconId;
            }
            else
            {
                localNotification.IconId = Resource.Drawable.notification_template_icon_bg;
            }

            var serializedNotification = SerializeNotification(localNotification);
            intent.PutExtra(DebugScheduledAlarmHandler.LocalNotificationKey, serializedNotification);

            Random generator = new Random();
            _randomNumber = generator.Next(100000, 999999).ToString("D6");

            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, Convert.ToInt32(_randomNumber), intent, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();

            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, 3000, pendingIntent);
        }

        public void Cancel(int id)
        {

            var intent = CreateIntent(id);
            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, Convert.ToInt32(_randomNumber), intent, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();
            alarmManager.Cancel(pendingIntent);
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.CancelAll();
            notificationManager.Cancel(id);
        }

        public static Intent GetLauncherActivity()
        {

            var packageName = Application.Context.PackageName;
            return Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
        }


        private Intent CreateIntent(int id)
        {

            return new Intent(Application.Context, typeof(DebugScheduledAlarmHandler))
                .SetAction("LocalNotifierIntent" + id);
        }

        private AlarmManager GetAlarmManager()
        {

            var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            return alarmManager;
        }

        private string SerializeNotification(LocalNotification notification)
        {

            var xmlSerializer = new XmlSerializer(notification.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, notification);
                return stringWriter.ToString();
            }
        }
    }

    [BroadcastReceiver(Enabled = true, Label = "Debug Noti")]
    public class DebugScheduledAlarmHandler : BroadcastReceiver
    {

        public const string LocalNotificationKey = "LocalNotification";
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        NotificationManager manager;
        NotificationCompat.BigTextStyle textStyle = new NotificationCompat.BigTextStyle();
        Dictionary<string,int> nQueue = Global.notifQueue;


        private SQLiteConnection _sqLiteConnection ;



        public override async void OnReceive(Context context, Intent intent)
        {
            _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
            var listDataPower = _sqLiteConnection.Table<Power>().ToList();

            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            intent.SetFlags(ActivityFlags.SingleTop);
            intent.PutExtra("OpenPage", "SomePage");


            var extra = intent.GetStringExtra(LocalNotificationKey);
            var notification = DeserializeNotification(extra);

            int notiID = notification.Index;


            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(listDataPower[notiID].Title)
                .SetContentText(listDataPower[notiID].Description)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.notify_panel_notification_icon_bg))
                .SetSmallIcon(Resource.Drawable.notify_panel_notification_icon_bg)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetStyle(textStyle);

            var resultIntent = DebugNoti.GetLauncherActivity();
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Application.Context);
            stackBuilder.AddNextIntent(resultIntent);

            Random random = new Random();
            int randomNumber = random.Next(9999 - 1000) + 1000;

            var resultPendingIntent =
                stackBuilder.GetPendingIntent(randomNumber, (int)PendingIntentFlags.Immutable);
            builder.SetContentIntent(resultPendingIntent);
            // Sending notification    
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Notify(randomNumber, builder.Build());

            notiID++;

            Xamarin.Forms.DependencyService.Get<IDebugNotiServ>().LocalNotification(listDataPower[notiID].Title, listDataPower[notiID].Description, notiID, DateTime.Now);
        }

        private LocalNotification DeserializeNotification(string notificationString)
        {

            var xmlSerializer = new XmlSerializer(typeof(LocalNotification));
            using (var stringReader = new StringReader(notificationString))
            {
                var notification = (LocalNotification)xmlSerializer.Deserialize(stringReader);
                return notification;
            }
        }

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