using LawsForImpact.DebugTest;
using LawsForImpact.Services;
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

           
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CountFunctionOn();
        }

        void CountFunctionOn()
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Enabled = true;
            timer.Start();
            timer.Elapsed += CountEvent;
        }

        private string x;

        private int counter;
        public int Counter
        {
            get => counter;
            set
            {
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
            DependencyService.Get<IReminderService>().Remind(DateTime.Now, "INSERT MY TITLE","INSERT MY MESSAGE");
        }
    }
}