﻿using System;

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

            var intent = new Intent(this, typeof(PeriodicService));
            StartService(intent);

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //OnNewIntent(Intent);
        }

        //protected override void OnNewIntent(Intent intent)
        //{
            

        //    // Send message to the PCL (XF) if a certain page should be opened.
        //    if (intent.HasExtra("OpenPage"))
        //    {
        //        string pageName = intent.GetStringExtra("OpenPage") ?? "None";

        //        if (pageName != "None")
        //        {
        //            var message = new OpenPageMessage { PageName = pageName };
        //            MessagingCenter.Send(message, Message.Msg_OpenPage);


        //        }
        //    }

        //    base.OnNewIntent(intent);
        //    //CreateNotificationFromIntent(Intent);
        //}
        
        //void CreateNotificationFromIntent(Intent intent)
        //{
        //    if (intent?.Extras != null)
        //    {
        //        string title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
        //        string message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);
        //        DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
        //    }
        //}
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}