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

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            //MainPage = new DebugBackgroundCounter();
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
