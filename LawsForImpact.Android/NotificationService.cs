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

        public void LocalNotification(string title, string body, int id, DateTime notifyTime)
        {
            // !Todo change the repeated length, connect the notification interval switch with these
            //long repeateDay = 1000 * 60 * 60 * 24;    
            // long repeateForMinute = 60000; // In milliseconds  
            var selectedInterval = new NotificationViewModel();
            long selectedRepeat = 0;

            // todo change to actual time, using 1ms for debugging
            if (selectedInterval.EverydayToggle == true) 
            {
                selectedRepeat = 1;
                // every 24 hr
                //selectedRepeat = 1000 * 60 * 60 * 24;
                //// this is one days worth 
                //// 1000ms -> 1s*60 = 60s -> 1m*60=60m -> 1h*24=24h

            }
            else if(selectedInterval.OtherDayToggle == true)
            {
                selectedRepeat = 1;
                //// every 48hr
                //selectedRepeat = 1000 * 60 * 60 * 24 * 2; 
            }
            else if(selectedInterval.WeeklyToggle == true)
            {
                selectedRepeat = 1;
                //// every 168hr -> 1 week
                //selectedRepeat = 1000 * 60 * 60 * 24 * 7;
            }
            else if(selectedInterval.MonthlyToggle == true)
            {
                selectedRepeat = 1;
                //// monthlys worth, 672 hrs, 28 days
                //selectedRepeat = 2419200000;
            }


            long totalMilliSeconds = (long)(notifyTime.ToUniversalTime() - _jan1st1970).TotalMilliseconds;
            if (totalMilliSeconds <JavaSystem.CurrentTimeMillis())
            {
                totalMilliSeconds = totalMilliSeconds + selectedRepeat;
            }

            var intent = CreateIntent(id);
            var localNotification = new LocalNotification();
            localNotification.Title = title;
            localNotification.Body = body;
            localNotification.Id = id;
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
            intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serializedNotification);

            Random generator = new Random();
            _randomNumber = generator.Next(100000, 999999).ToString("D6");

            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, Convert.ToInt32(_randomNumber), intent, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();
            // todo change variable of alarm manager
            //totalMilliSeconds, repeateForMinute
            alarmManager.SetRepeating(AlarmType.RtcWakeup, 1, 1, pendingIntent);
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
        Dictionary<string,int> nQueue = Global.notifQueue;


        private SQLiteConnection _sqLiteConnection;
        string ruleIndex;
        string title;
        string message;

        // determine next notification
        private int notifIterator(int elementMax)
        {
            //SOLVED: War skips exponentially
            Global.whichElement++;
            if (Global.whichElement >= nQueue.Count)
            {
                // if all elements have gone through once, starts back to first element
                Global.whichElement = 0;
            }


            
            // SOLVED: issue is elementMax and chosenElement dont align 

            
            string choseNotifElement = nQueue.ElementAt(Global.whichElement).Key;
            Global.notifCurrentTitle = choseNotifElement;

            // checks whether current table index overflows, resets to starting point if yes
            if (nQueue[choseNotifElement] < 0)
            {
                nQueue[choseNotifElement] = elementMax - 1;
            }

            // next index chosen
            int returnedIndex = elementMax - nQueue[choseNotifElement];
            Global.notifCurrentIndex = returnedIndex;
            // todo figure out what happens when queue number reaches 0
            nQueue[choseNotifElement] = nQueue[choseNotifElement] - 1;

            // next notification table on the list
            int nextNotifIndex = Global.whichElement + 1;
            if (nextNotifIndex < nQueue.Count)
            {
                Global.notifTitle = nQueue.ElementAt(nextNotifIndex).Key;
            }
            else
            {
                Global.notifTitle = nQueue.ElementAt(0).Key;
            }

            

            //todo restart the value count if zero is hit
            return returnedIndex-1;

        }
        int iteratedIndex;
        private async void RefreshListView()
        {
            try
            {
                // where the cooridination of which title to use
                // TODO how notifications come out
                switch (Global.notifTitle)
                {
                    case "Power":
                        _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
                        var listDataPower = _sqLiteConnection.Table<Power>().ToList();

                        // inserts info of the max index count of table
                         iteratedIndex = notifIterator(_sqLiteConnection.Table<Power>().Count());

                        ruleIndex = $"Law {iteratedIndex + 1}";
                        title = listDataPower[iteratedIndex].Title;
                        message = listDataPower[iteratedIndex].Description;
                        Global.notifDescription = title;
                        Global.notifFullDescrip = message;
                        break;
                    case "Mastery":
                        _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
                        var listDataMastery = _sqLiteConnection.Table<Mastery>().ToList();

                        // inserts info of the max index count of table
                         iteratedIndex = notifIterator(_sqLiteConnection.Table<Mastery>().Count());

                        ruleIndex = $"Principle {iteratedIndex + 1}";
                        title = listDataMastery[iteratedIndex].Title;
                        message = listDataMastery[iteratedIndex].Description;
                        Global.notifDescription = title;
                        Global.notifFullDescrip = message;
                        break;
                    case "War":
                        _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
                        var listDataWar = _sqLiteConnection.Table<War>().ToList();

                        iteratedIndex = notifIterator(_sqLiteConnection.Table<War>().Count());

                        ruleIndex = $"Law {iteratedIndex + 1}";
                        title = listDataWar[iteratedIndex].Title;
                        message = listDataWar[iteratedIndex].Description;
                        Global.notifDescription = title;
                        Global.notifFullDescrip = message;
                        break;
                    case "Friends":
                        _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
                        var listDataFriends = _sqLiteConnection.Table<Friends>().ToList();

                        iteratedIndex = notifIterator(_sqLiteConnection.Table<Friends>().Count());

                        ruleIndex = $"Law {iteratedIndex + 1}";
                        title = listDataFriends[iteratedIndex].Title;
                        message = listDataFriends[iteratedIndex].Description;
                        Global.notifDescription = title;
                        Global.notifFullDescrip = message;
                        break;
                    case "Human":
                        _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
                        var listDataHuman = _sqLiteConnection.Table<Human>().ToList();

                        iteratedIndex = notifIterator(_sqLiteConnection.Table<Human>().Count());

                        ruleIndex = $"Law {iteratedIndex + 1}";
                        title = listDataHuman[iteratedIndex].Title;
                        message = listDataHuman[iteratedIndex].Description;
                        Global.notifDescription = title;
                        Global.notifFullDescrip = message;
                        break;
                    case "User":
                        _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
                        var listDataUser = _sqLiteConnection.Table<User>().ToList();

                        iteratedIndex = notifIterator(_sqLiteConnection.Table<User>().Count());

                        ruleIndex = $"Reminder {iteratedIndex + 1}";
                        title = listDataUser[iteratedIndex].Title;
                        message = listDataUser[iteratedIndex].Description;
                        Global.notifDescription = title;
                        Global.notifFullDescrip = message;
                        break;
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }

        }
        

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

            // TODO issue is the count keeps going up and doesnt restart
            Global.count++;

            RefreshListView();

            var extra = intent.GetStringExtra(LocalNotificationKey);
            var notification = DeserializeNotification(extra);
            //Generating notification    
            notificationNumber++;

            

            textStyle.BigText(message);

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