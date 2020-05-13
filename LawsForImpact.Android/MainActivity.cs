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
using Xamarin.Essentials;
using LawsForImpact.Models;

namespace LawsForImpact.Droid
{
    [Activity(
        LaunchMode = LaunchMode.SingleTop, 
        Label = "Influence",
        Icon = "@drawable/check_logo", 
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
       
            CreateNotificationFromIntent(Intent);

        }

        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string tableKey = intent.Extras.GetString(AndroidNotificationManager.TableKey);
                int indexKey = intent.Extras.GetInt(AndroidNotificationManager.IndexKey);

                DependencyService.Get<INotificationManager>().ReceiveNotification(tableKey, indexKey);
            }
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}