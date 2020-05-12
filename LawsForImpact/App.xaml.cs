using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LawsForImpact.Services;
using LawsForImpact.Views;
using LawsForImpact.Models;
using SQLite;

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



        
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            MainPage = new MainPage();
            
            
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
