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
using LawsForImpact.Views;
using SQLite;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

namespace LawsForImpact.Droid
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver  
    {
        const string channelId = "default";
        public override void OnReceive(Context context, Intent intent)
        {

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

            var notification = builder.Build();
            manager.Notify(0, notification);

            // this seems to work when the above notify doesnt work

            // var resultIntent = NotificationService.GetLauncherActivity();
            //resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            //var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Application.Context);
            //stackBuilder.AddNextIntent(resultIntent);
            //Random random = new Random();
            //int randomNumber = random.Next(9999 - 1000) + 1000;

            //var resultPendingIntent =
            //    stackBuilder.GetPendingIntent(randomNumber, (int)PendingIntentFlags.Immutable);
            //builder.SetContentIntent(resultPendingIntent);
            //// Sending notification    
            //var notificationManager = NotificationManagerCompat.From(Application.Context);
            //notificationManager.Notify(randomNumber, builder.Build());

        }
    }
}