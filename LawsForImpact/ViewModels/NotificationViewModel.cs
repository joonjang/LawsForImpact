using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using LawsForImpact.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace LawsForImpact.ViewModels
{
    public class NotificationViewModel : BaseViewModel
    {
        public NotificationViewModel()
        {
            SaveCommand = new Command(() => SaveLocalNotification());
            Title = "Notification";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }

        Command _saveCommand;
        public Command SaveCommand
        {
            get
            {
                return _saveCommand;
            }
            set
            {
                SetProperty(ref _saveCommand, value);
            }
        }

        DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                SetProperty(ref _selectedDate, value);
            }
        }
        TimeSpan _selectedTime = DateTime.Now.TimeOfDay;
        public TimeSpan SelectedTime
        {
            get
            {
                return _selectedTime;
            }
            set
            {
                SetProperty(ref _selectedTime, value);
            }
        }

        void SaveLocalNotification()
        {
                var date = (SelectedDate.Date.Month.ToString("00") + "-" + SelectedDate.Date.Day.ToString("00") + "-" + SelectedDate.Date.Year.ToString());
                var time = Convert.ToDateTime(SelectedTime.ToString()).ToString("HH:mm");
                var dateTime = date + " " + time;
                var selectedDateTime = DateTime.ParseExact(dateTime, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);

                DependencyService.Get<INotificationService>().Cancel(0);
                DependencyService.Get<INotificationService>().LocalNotification("Local Notification", "BODY MESSAGE", 0, selectedDateTime);
                App.Current.MainPage.DisplayAlert("LocalNotificationDemo", "Notification details saved successfully ", "Ok");

        }
        //protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
        //        return false;
        //    backingStore = value;
        //    onChanged?.Invoke();
        //    OnPropertyChanged(propertyName);
        //    return true;
        //}
        // Todo modify this, its a hidden method
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    var changed = PropertyChanged;
        //    if (changed == null)
        //        return;
        //    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}