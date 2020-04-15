using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LawsForImpact.Services;
using LawsForImpact.Views;
using LawsForImpact.Models;
using LawsForImpact.ViewModels;
using System.Diagnostics;

namespace LawsForImpact
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();

            Debug.WriteLine("IN THE FRONT");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("IN THE BACKGROUND");
        }

        protected override void OnResume()
        {
        }
    }
}
