using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using LawsForImpact.Models;
using LawsForImpact.Services;
using SQLite;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace LawsForImpact.Droid
{
    [BroadcastReceiver]

    public class BackgroundReceiver : BroadcastReceiver
    {
        const string channelId = "default";
        private SQLiteConnection _sqLiteConnection;
        string title;
        string message;
        int iteratedIndex;

        // from https://xamarinhelp.com/xamarin-background-tasks/
        public override async void OnReceive(Context context, Intent intent)
        {
            //PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            //PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "BackgroundReceiver");

            ////wakeLock.Acquire();


            //var alarmIntent = new Intent(context, typeof(BackgroundReceiver));

            //var pending = PendingIntent.GetBroadcast(context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            //var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            //alarmManager.SetExactAndAllowWhileIdle(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 1 * 1000, pending);

            ////MessagingCenter.Send<object, string>(this, "UpdateLabel", "Hello from Android");
            //MessagingCenter.Send<object>(this, "SendNotification");


            /////




            //Intent alarmIntent = new Intent(Application.Context, typeof(AlarmReceiver));
            //alarmIntent.PutExtra("message", message);
            //alarmIntent.PutExtra("title", title);

            //// specify the broadcast receiver
            //PendingIntent pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            //AlarmManager alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);

            //// set the time when app is woken up
            //// todo: this is where time is adjusted
            //alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 5 * 1000, 1000, pendingIntent);


            _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
            var listDataPower = _sqLiteConnection.Table<Power>().ToList();

            iteratedIndex++;

            //title = listDataPower[iteratedIndex].Title;
            //message = listDataPower[iteratedIndex].Description;



            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");


            var notIntent = new Intent(context, typeof(MainActivity));
            var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
            var manager = NotificationManagerCompat.From(context);

            var style = new NotificationCompat.BigTextStyle();
            style.BigText(message);

            // sets notifcation logo
            int resourceId;
            resourceId = Resource.Drawable.xamarin_logo;

            var wearableExtender = new NotificationCompat.WearableExtender().SetBackground(BitmapFactory.DecodeResource(context.Resources, resourceId));



            // Generate a notification
            // todo look at notification compat properties
            var builder = new NotificationCompat.Builder(context, channelId)
                .SetContentIntent(contentIntent)
                .SetSmallIcon(Resource.Drawable.notification_template_icon_bg)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetStyle(style)
                .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
                .SetAutoCancel(true)
                .Extend(wearableExtender);


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

            //var notification = builder.Build();
            //manager.Notify(0, notification);

            /////////////////////////////////////////////////////////
            ///

            //wakeLock.Release();

            //////////////////////////////////////////////////////////////////////
        }
    }
}