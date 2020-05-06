using System;
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

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = -1;
        NotificationManager manager;

        public event EventHandler NotificationReceived;
        string currentTitle;
        bool randomBool;
        SerializableDictionary<string, int> nQueue;

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

            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ic_mtrl_chip_checked_circle))
                .SetSmallIcon(Resource.Drawable.ic_mtrl_chip_close_circle)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            manager.Notify(messageId, notification);


            RepeatAlarmSet();

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
            ScheduleNotification("second notification", "wowowowo");
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


        //public void SavedInfo(SerializableDictionary<string, int> pickedQueue, int queueIndex, int index, bool randomTog, int repeatInterval)
        //{
        //    currentTitle = pickedQueue.ElementAt(queueIndex).Key;
        //    randomBool = randomTog;
        //    nQueue = pickedQueue;
        //}

        //private async void LoadData()
        //{

        //    //SOLVED: go over how inherited base interface can become the generic list 
        //    // so far IEnumerable can hold the different list type, but why
        //    // enumerable is just a general blanket of lazily getting information, converting that to a more 
        //    // structured type seemed to do the trick
        //    _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
        //    IEnumerable<IDataTable> tableToEnumerable = new List<IDataTable>();
        //    List<IDataTable> listData;

        //    switch (currentTitle)
        //    {
        //        case "Power":
        //            tableToEnumerable = _sqLiteConnection.Table<Power>().ToList();
        //            break;
        //        case "Mastery":
        //            tableToEnumerable = _sqLiteConnection.Table<Mastery>().ToList();
        //            break;
        //        case "User":
        //            tableToEnumerable = _sqLiteConnection.Table<User>().ToList();
        //            break;
        //        case "War":
        //            tableToEnumerable = _sqLiteConnection.Table<War>().ToList();
        //            break;
        //        case "Friends":
        //            tableToEnumerable = _sqLiteConnection.Table<Friends>().ToList();
        //            break;
        //        case "Human":
        //            tableToEnumerable = _sqLiteConnection.Table<Human>().ToList();
        //            break;
        //    }
        //    listData = tableToEnumerable.ToList();


        //    int tmp = nQueue[currentTitle];

        //    int index = listData.Count() - nQueue[currentTitle];
        //    index = index - 1;


        //    // if random enabled
        //    if (randomBool)
        //    {
        //        Random random = new Random();
        //        index = random.Next(0, listData.Count());
        //    }


        //    // sets all the current notification information
        //    title = listData[index].Title;
        //    message = listData[index].Description;

        //    //logic for next notification


        //    // subtract the queue int of current notification subject to keep track of next index
        //    nQueue[currentTitle] = nQueue[currentTitle] - 1;

        //    // check for index overflow
        //    if (nQueue[currentTitle] < 0)
        //    {
        //        nQueue[currentTitle] = listData.Count() - 1;
        //    }


        //    // index of next table
        //    nextElementIndex = currentElementIndex + 1;


        //    // if next table index overflows that means its time to restart the table index and move up the notification index
        //    if (nextElementIndex >= nQueue.Count)
        //    {
        //        nextElementIndex = 0;
        //    }


        //    nextNotifTableName = nQueue.ElementAt(nextElementIndex).Key;

        //}
    }
}