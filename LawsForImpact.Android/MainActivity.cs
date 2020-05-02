using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using LawsForImpact.Services;
using Xamarin.Forms;
using LawsForImpact.Views;
using Android;


namespace LawsForImpact.Droid
{
    [Activity(
        LaunchMode = LaunchMode.SingleTop, 
        Label = "LawsForImpact",
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());

            ////var intent = new Intent(this, typeof(PeriodicService));
            ////StartService(intent);

            //var alarmIntent = new Intent(this, typeof(BackgroundReceiver));

            //var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            //var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            ////AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);

            //alarmManager.SetRepeating(AlarmType.RtcWakeup, 1, 1, pending);
            ////alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 1 * 1000, 1000, pending);
            ////alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + 1 * 1000, pending);
            
        }

       

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}