using System;
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

[assembly: Xamarin.Forms.Dependency(typeof(NotificationService))]
namespace LawsForImpact.Droid
{
    class NotificationService : INotificationService
    {
        int _notificationIconId { get; set; }
        readonly DateTime _jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal string _randomNumber;

        public void LocalNotification(int id, DateTime notifyTime, int queueIndex, SerializableDictionary<string, int> notificationQueue, bool randomToggle, long nextRepeat = 3000)
        {
            // !Todo change the repeated length, connect the notification interval switch with these
            //long repeateDay = 1000 * 60 * 60 * 24;    
            // long repeateForMinute = 60000; // In milliseconds  
            var selectedInterval = new NotificationViewModel();
            long selectedRepeat = nextRepeat;

            // todo change to actual time, using 1ms for debugging
            if (selectedInterval.EverydayToggle == true) 
            {
                selectedRepeat = 1000;
                // every 24 hr
                //selectedRepeat = 1000 * 60 * 60 * 24;
                //// this is one days worth 
                //// 1000ms -> 1s*60 = 60s -> 1m*60=60m -> 1h*24=24h

            }
            else if(selectedInterval.OtherDayToggle == true)
            {
                selectedRepeat = 1000;
                //// every 48hr
                //selectedRepeat = 1000 * 60 * 60 * 24 * 2; 
            }
            else if(selectedInterval.WeeklyToggle == true)
            {
                selectedRepeat = 1000;
                //// every 168hr -> 1 week
                //selectedRepeat = 1000 * 60 * 60 * 24 * 7;
            }
            else if(selectedInterval.MonthlyToggle == true)
            {
                selectedRepeat = 1000;
                //// monthlys worth, 672 hrs, 28 days
                //selectedRepeat = 2419200000;
            }


            long totalMilliSeconds = (long)(notifyTime.ToUniversalTime() - _jan1st1970).TotalMilliseconds;
            if (totalMilliSeconds <JavaSystem.CurrentTimeMillis())
            {
                totalMilliSeconds = totalMilliSeconds + selectedRepeat;
            }

            // id might cause problem because im using it as index and intent is using as id, index is going to repeat, is that ok
            var intent = CreateIntent(id);
            var localNotification = new LocalNotification();
            //localNotification.Title = title;
            //localNotification.Body = body;
            //localNotification.Index = id;
            localNotification.NotifyTime = notifyTime;
            localNotification.RepeatInterval = selectedRepeat;
            localNotification.QueueIndex = queueIndex;
            localNotification.NotificationQueue = notificationQueue;
            localNotification.RandomToggle = randomToggle;


            if (_notificationIconId != 0)
            {
                localNotification.IconId = _notificationIconId;
            }
            else
            {
                localNotification.IconId = Resource.Drawable.notification_template_icon_bg;
            }

            var serializedNotification = SerializeNotification(localNotification);
            intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serializedNotification);

            Random generator = new Random();
            _randomNumber = generator.Next(100000, 999999).ToString("D6");

            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, Convert.ToInt32(_randomNumber), intent, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();
            // todo change variable of alarm manager
            //totalMilliSeconds, repeateForMinute
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, totalMilliSeconds, pendingIntent);
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

            return new Intent(Application.Context, typeof(ScheduledAlarmHandler))
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

    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {

        public const string LocalNotificationKey = "LocalNotification";
        int notificationNumber = -1;
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        NotificationManager manager;
        NotificationCompat.BigTextStyle textStyle = new NotificationCompat.BigTextStyle();
        SerializableDictionary<string,int> nQueue;


        private SQLiteConnection _sqLiteConnection;
        string ruleIndex;
        string title;
        string message;
        int iteratedIndex;
        bool randomBool;


        // current stuff
        int currentElementIndex;
        string currentTitle;

        // next information
        string nextNotifTableName;
        int nextElementIndex;


        public override void OnReceive(Context context, Intent intent)
        {

            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }
            //RefreshListView(); // i deleted this and thats where it all went wrong
            // turns out it was from a switch statement in the ItemsPage

            intent.SetFlags(ActivityFlags.SingleTop);
            intent.PutExtra("OpenPage", "SomePage");


            var extra = intent.GetStringExtra(LocalNotificationKey);
            var notification = DeserializeNotification(extra);
            //Generating notification    
            notificationNumber++;

            nQueue = notification.NotificationQueue;
            currentElementIndex = notification.QueueIndex;
            currentTitle = notification.NotificationQueue.ElementAt(currentElementIndex).Key;
            randomBool = notification.RandomToggle;

            LoadData();

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.notify_panel_notification_icon_bg))
                .SetSmallIcon(Resource.Drawable.notify_panel_notification_icon_bg)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetStyle(textStyle);

            var resultIntent = NotificationService.GetLauncherActivity();
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

            Xamarin.Forms.DependencyService.Get<INotificationService>().LocalNotification(0,DateTime.Now, nextElementIndex, nQueue, randomBool, notification.RepeatInterval);
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


            int tmp = nQueue[currentTitle];

            int index = listData.Count() - nQueue[currentTitle];
            index = index - 1;


            // if random enabled
            if (randomBool)
            {
                Random random = new Random();
                index = random.Next(0, listData.Count());
            }


            // sets all the current notification information
            title = listData[index].Title;
            message = listData[index].Description;

            //logic for next notification


            // subtract the queue int of current notification subject to keep track of next index
            nQueue[currentTitle] = nQueue[currentTitle] - 1;

            // check for index overflow
            if (nQueue[currentTitle] < 0)
            {
                nQueue[currentTitle] = listData.Count() - 1;
            }


            //if (nQueue[currentTitle] == 0)
            //{
            //    nQueue[currentTitle] = listData.Count() - 1;
            //}

            // index of next table
            nextElementIndex = currentElementIndex + 1;
            

            // if next table index overflows that means its time to restart the table index and move up the notification index
            if(nextElementIndex >= nQueue.Count)
            {
                nextElementIndex = 0;
            }

            // nextIndex for example:
            // Power has 48 counts which is the elementMax, nQueue[Power] start with 47
            // elementMax - nQueue[Power]
            // 48 - 47 = 1 which is the index (make sure to subtract by 1)
            // nQueue[Power] = 47 --> nQueue[Power] = 46
            // next iteration
            // 48 - 46 = 2 and so on...
            //
            // Last index occasion, nQueue[Power] = 0
            // 48 - 0 = 48 and we subtract by 1 so the next index is set to -1
            // so restart the nQueue[Power] count once it reach 0
            // nQueue[Power] = maxElement - 1 --> 48 - 1 = 47

            //PeakNextTableElementMax();

            nextNotifTableName = nQueue.ElementAt(nextElementIndex).Key;
            //nextIndex = nextElementMax - nQueue[nextNotifTableName];

            //if (nQueue[nextNotifTableName] == 0)
            //{
            //    nQueue[nextNotifTableName] = nextElementMax - 1;
            //}


        }
        //int nextElementMax;
        //private async void PeakNextTableElementMax()
        //{
        //    _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
        //    IEnumerable<IDataTable> tableToEnumerable = new List<IDataTable>();
        //    List<IDataTable> listData;

        //    switch (nextNotifTableName)
        //    {
        //        case "Power":
        //            nextElementMax = _sqLiteConnection.Table<Power>().ToList().Count();
        //            break;
        //        case "Mastery":
        //            nextElementMax = _sqLiteConnection.Table<Mastery>().ToList().Count();
        //            break;
        //    }

        //}


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