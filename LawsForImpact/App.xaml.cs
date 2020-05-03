using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LawsForImpact.Services;
using LawsForImpact.Views;
using LawsForImpact.Models;

namespace LawsForImpact
{
    public partial class App : Application
    {

        /// <summary>
        /// I have solved the issue of the app crashing when open the notification when the app is running in the foreground. I have 
        /// isolated the issue be occuring from the Initilization of the MainPage. I have the initilization of the MainPage also cancels
        /// all notification activity. That is an issue that I will need to solve. Once I sold that issue, I believe the notification
        /// crashing would inadvertantly be solved as well as I belive they are linked.
        /// </summary>



        INotificationService notificationService;
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Get<INotificationService>().Initialize();
            //MainPage = new DebugBackgroundCounter();

            notificationService = DependencyService.Get<INotificationService>();
            notificationService.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                Global.notifTitle = evtData.Title;
                Global.notifDescription = evtData.Message;
            };

            if (Global.notifTitle != null)
            {
                string tmp = Global.notifDescription;
                int tmp1 = Global.notifCurrentIndex;
                string tmp2 = Global.notifFullDescrip;
                string tmp3 = Global.notifCurrentTitle;

                MainPage = new ItemDetailPage();
            }
            else
            {
                MainPage = new MainPage();
            }
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            //MainPage = new ItemDetailPage();
        }
    }
}
