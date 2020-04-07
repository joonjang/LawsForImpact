using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using LawsForImpact.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using LawsForImpact.Models;

namespace LawsForImpact.ViewModels
{
    public class NotificationViewModel : BaseViewModel
    {
        public NotificationViewModel()
        {
            SaveCommand = new Command(() => SaveLocalNotification());
            Title = "Notification";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
            HeaderTitle = Global.notifTitle;
        }

        private string headerTitle;
        public string HeaderTitle
        {
            get { return headerTitle; }
            set
            {
                headerTitle = value;
                OnPropertyChanged(nameof(HeaderTitle));
            }
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
        // todo go over why this works
        private bool everydayToggle;
        public bool EverydayToggle
        {
            get => everydayToggle;
            set
            {

                SetProperty(ref everydayToggle, value, nameof(EverydayToggle));

                if (value)
                {
                    OtherDayToggle = false;
                    WeeklyToggle = false;
                    MonthlyToggle = false;

                }
            }
        }

        // todo find out how this value parameter works
        private bool showDate;
        public bool ShowDate
        {
            get => showDate;
            set
            {
                SetProperty(ref showDate, value, nameof(ShowDate));
            }
        }

        private bool otherDayToggle;
        public bool OtherDayToggle
        {
            get => otherDayToggle;
            set
            {
                SetProperty(ref otherDayToggle, value, nameof(OtherDayToggle));

                if (value)
                {
                    EverydayToggle = false;
                    WeeklyToggle = false;
                    MonthlyToggle = false;
                    ShowDate = true;
                    
                }
                else
                {
                    ShowDate = false;
                }
            }
        }

        private bool weeklyToggle;
        public bool WeeklyToggle
        {
            get => weeklyToggle;
            set
            {

                SetProperty(ref weeklyToggle, value, nameof(WeeklyToggle));
                if (value)
                {
                    EverydayToggle = false;
                    OtherDayToggle = false;
                    MonthlyToggle = false;
                    ShowDate = true;

                }
                else
                {
                    ShowDate = false;
                }
            }
        }

        private bool monthlyToggle;
        public bool MonthlyToggle
        {
            get => monthlyToggle;
            set
            {

                SetProperty(ref monthlyToggle, value, nameof(MonthlyToggle));
                if (value)
                {
                    EverydayToggle = false;
                    OtherDayToggle = false;
                    WeeklyToggle = false;
                    ShowDate = true;

                }
                else
                {
                    ShowDate = false;
                }
            }
        }


        //private bool selectAll;
        //public bool SelectAll
        //{
        //    get => selectAll;
        //    set
        //    {
        //        SetProperty(ref selectAll, value, nameof(SelectAll));

        //        if (value)
        //        {
        //            Option1 = true;
        //            Option2 = true;
        //            Option3 = true;
        //            Option4 = true;
        //        }
        //    }
        //}

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