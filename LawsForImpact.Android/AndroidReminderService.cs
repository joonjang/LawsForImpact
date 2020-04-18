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
using LawsForImpact.Droid;
using LawsForImpact.Services;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidReminderService))]
namespace LawsForImpact.Droid
{

    //used: https://blog.nishanil.com/2014/12/16/scheduled-notifications-in-android-using-alarm-manager/
    // new notifications service
    public class AndroidReminderService : IReminderService
    {
        public void Remind(DateTime dateTime, string title, string message)
        {


            // create alarm intent
            Intent alarmIntent = new Intent(Application.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("message", message);
            alarmIntent.PutExtra("title", title);

            // specify the broadcast receiver
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);

            // set the time when app is woken up
            // todo: this is where time is adjusted
            alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 5* 1000, 1000, pendingIntent);



            ////Intent alarmIntent = new Intent(this, typeof(AlarmReceiver));

            ////PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            ////AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);
            //var tomorrowMorningTime = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 1);
            //var timeDifference = tomorrowMorningTime – DateTime.Now;
            //var millisecondsInOneDay = 86400000;
            //alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + (long)timeDifference.TotalMilliseconds, (long)millisecondsInOneDay, pendingIntent);
        }

    }
}