using LawsForImpact.DebugTest;
using LawsForImpact.Services;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LawsForImpact.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class DebugBackgroundCounter : ContentPage
    {

       
        Timer timer;
        public DebugBackgroundCounter()
        {
            InitializeComponent();
            BindingContext = this;


            //MessagingCenter.Subscribe<object, string>(this, "UpdateLabel", (s, e) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        BackgroundServiceLabel.Text = e;
            //    });
            //});

            MessagingCenter.Subscribe<object>(this, "SendNotification", (s) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossLocalNotifications.Current.Cancel(0);
                    CrossLocalNotifications.Current.Show("Alarm", "BZEM notification time", 0, DateTime.Now) ;
                });
            });

        }

        private bool wake;
        public bool Wake
        {
            get => wake;
            set
            {
                wake = value;
                SetProperty(ref wake, value, nameof(Wake));
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CountFunctionOn();
        }

        public void CountFunctionOn()
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Enabled = true;
            timer.Start();
            timer.Elapsed += CountEvent;
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                SetProperty(ref message, value, nameof(Message));
            }
        }

       

        private int counter;
        public int Counter
        {
            get => counter;
            set
            {
                Message = counter.ToString();
                SetProperty(ref counter, value, nameof(Counter));
            }
        }

        void CountEvent(object sender, EventArgs e)
        {
            Counter++;
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            timer.Stop();
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            DependencyService.Get<IReminderService>().Remind(DateTime.Now, "INSERT MY TITLE","INSERT MY MES" +
                "asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaSAGE");
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {

                DependencyService.Get<IDebugNotiServ>().Cancel(0);
                DependencyService.Get<IDebugNotiServ>().LocalNotification("Local Notification", "BODY MESSAGE", 0, DateTime.Now);
                App.Current.MainPage.DisplayAlert("Noti title from debug bg page", "lets fucking gettit", "LETS FUCKING WIN");

        }
    }
}