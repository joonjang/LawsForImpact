﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Graphics;

using Android.Support.V4.App;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;
using Application = Android.App.Application;
using LawsForImpact.Services;
using LawsForImpact.Models;
using Java.Lang;
using SQLite;
using System.IO;
using System.Xml.Serialization;

[assembly: Dependency(typeof(LawsForImpact.Droid.AndroidNotificationManager))]
namespace LawsForImpact.Droid
{
    [BroadcastReceiver]
    public class AndroidNotificationManager : BroadcastReceiver, INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;
        private SQLiteConnection _sqLiteConnection;
        public const string LocalNotificationKey = "LocalNotification";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = -1;
        NotificationManager manager;

        public event EventHandler NotificationReceived;

        string currentTitle;
        SavedInformation savedInfo;

        public void Initialize()
        {
            CreateNotificationChannel();
        }

        public int ScheduleNotification(string title, string message)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            messageId++;


            // 1 MainActivity intent allows MainActivity to change once notification tapped
            Intent intentMain = new Intent(AndroidApp.Context, typeof(MainActivity));
            // 2 the alarm repeater
            Intent intentAndroid = new Intent(AndroidApp.Context, typeof(AndroidNotificationManager));


            intentMain.PutExtra(TitleKey, title);
            intentMain.PutExtra(MessageKey, message);


            var serializedNotification = SerializeNotification(savedInfo);
            intentAndroid.PutExtra(LocalNotificationKey, serializedNotification);

            PendingIntent pendingIntentMain = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intentMain, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntentMain)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ic_mtrl_chip_checked_circle))
                .SetSmallIcon(Resource.Drawable.ic_mtrl_chip_close_circle)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            manager.Notify(messageId, notification);

            var pendingIntentAndroid = PendingIntent.GetBroadcast(Application.Context, 0, intentAndroid, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();
            // todo change variable of alarm manager
            //totalMilliSeconds, repeateForMinute
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, 3000, pendingIntentAndroid);
        

            ///////0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
            //var resultIntent = AndroidNotificationManager.GetLauncherActivity();
            //resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            //var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Application.Context);
            //stackBuilder.AddNextIntent(resultIntent);

            //var resultPendingIntent =
            //    stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.Immutable);
            //builder.SetContentIntent(resultPendingIntent);

            //// Sending notification    
            //var notificationManager = NotificationManagerCompat.From(Application.Context);
            //notificationManager.Notify(0, builder.Build());

            //Xamarin.Forms.DependencyService.Get<INotificationManager>().SavedInfo(new SerializableDictionary<string, int>() { { "Power", 0 } }, 0, 0, false, 3000);
            //////--------------------------------------------------------------------------------------------------------------------------------------------------------------

            return messageId;
        }


        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
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

        public override void OnReceive(Context context, Intent intent)
        {
            var extra = intent.GetStringExtra(LocalNotificationKey);
            var notification = DeserializeNotification(extra);

            SerializableDictionary<string, int> queue = notification.QueueOfSaved;
            var queueIndex = notification.QueueIndex;
            var index = notification.Index;
            var randTog = notification.RandomToggle;
            var repInterval = notification.RepeatInterval;

            SavedInfo(queue, queueIndex, index, randTog, repInterval);
        }

        private string SerializeNotification(SavedInformation notification)
        {

            var xmlSerializer = new XmlSerializer(notification.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, notification);
                return stringWriter.ToString();
            }
        }

        private SavedInformation DeserializeNotification(string notificationString)
        {

            var xmlSerializer = new XmlSerializer(typeof(SavedInformation));
            using (var stringReader = new StringReader(notificationString))
            {
                var notification = (SavedInformation)xmlSerializer.Deserialize(stringReader);
                return notification;
            }
        }

        public void RepeatAlarmSet()
        {
            Intent intent = new Intent(Application.Context, typeof(AndroidNotificationManager));
            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, 30000, pendingIntent);
        }

        private AlarmManager GetAlarmManager()
        {

            var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            return alarmManager;
        }


        public void SavedInfo(SerializableDictionary<string, int> pickedQueue, int queueIndex, int index, bool randomTog, int repeatInterval)
        {
            savedInfo = new SavedInformation();

            currentTitle = pickedQueue.ElementAt(queueIndex).Key;
            savedInfo.QueueOfSaved = pickedQueue;
            savedInfo.QueueIndex = queueIndex;
            savedInfo.Index = index;
            savedInfo.RandomToggle = randomTog;
            savedInfo.RepeatInterval = repeatInterval;

            LoadData();

        }

        private async void LoadData()
        {

            //SOLVED: go over how inherited base interface can become the generic list 
            // so far IEnumerable can hold the different list type, but why
            // enumerable is just a general blanket of lazily getting information, converting that to a more 
            // structured type seemed to do the trick
            _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
            IEnumerable<IDataTable> tableToEnumerable = new List<IDataTable>();
            List<IDataTable> listData;

            switch (currentTitle)
            {
                case "Power":
                    tableToEnumerable = _sqLiteConnection.Table<Power>().ToList();
                    break;
                case "Mastery":
                    tableToEnumerable = _sqLiteConnection.Table<Mastery>().ToList();
                    break;
                case "User":
                    tableToEnumerable = _sqLiteConnection.Table<User>().ToList();
                    break;
                case "War":
                    tableToEnumerable = _sqLiteConnection.Table<War>().ToList();
                    break;
                case "Friends":
                    tableToEnumerable = _sqLiteConnection.Table<Friends>().ToList();
                    break;
                case "Human":
                    tableToEnumerable = _sqLiteConnection.Table<Human>().ToList();
                    break;
            }
            listData = tableToEnumerable.ToList();



            int index = listData.Count() - savedInfo.QueueOfSaved[currentTitle];
            index = index - 1;


            // if random enabled
            if (savedInfo.RandomToggle)
            {
                Random random = new Random();
                index = random.Next(0, listData.Count());
            }


            // sets all the current notification information
            string title = listData[index].Title;
            string message = listData[index].Description;

            //logic for next notification


            // subtract the queue int of current notification subject to keep track of next index
            savedInfo.QueueOfSaved[currentTitle] = savedInfo.QueueOfSaved[currentTitle] - 1;

            // check for index overflow
            if (savedInfo.QueueOfSaved[currentTitle] < 0)
            {
                savedInfo.QueueOfSaved[currentTitle] = listData.Count() - 1;
            }


            // index of next table
            savedInfo.QueueIndex = savedInfo.QueueIndex + 1;


            // if next table index overflows that means its time to restart the table index and move up the notification index
            if (savedInfo.QueueIndex >= savedInfo.QueueOfSaved.Count)
            {
                savedInfo.QueueIndex = 0;
            }

            ScheduleNotification(title, message);
        }
    }
}