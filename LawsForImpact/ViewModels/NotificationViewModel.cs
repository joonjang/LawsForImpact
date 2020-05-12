﻿using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using LawsForImpact.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using LawsForImpact.Models;
using System.Linq;
using SQLite;
using LawsForImpact.Views;

namespace LawsForImpact.ViewModels
{
    // todo make save show up only once iteration is pressed
    public class NotificationViewModel : BaseViewModel
    {
        SerializableDictionary<string, int> nQueue = new SerializableDictionary<string, int>();
        INotificationManager notificationManager;
        long intervalChosen;

        public NotificationViewModel()
        {

            ///////
            SaveCommand = new Command(() => SaveLocalNotification());
            CancelCommand = new Command(() => CancelNotification());
            //////



            Title = "Notification";
            HeaderTitle = Global.notifTitle;
            userCheck = Preferences.Get("User", false);
            powerCheck = Preferences.Get("Power", false);
            warCheck = Preferences.Get("War", false);
            masteryCheck = Preferences.Get("Mastery", false);
            friendsCheck = Preferences.Get("Friends", false);
            humanCheck = Preferences.Get("Human", false);
            randomOff = Preferences.Get("RandomOff", true);
            randomOn = Preferences.Get("RandomOn", false);

            everydayToggle = Preferences.Get("Everyday", false);
            otherDayToggle = Preferences.Get("Otherday", false);
            weeklyToggle = Preferences.Get("Weekly", false);
            monthlyToggle = Preferences.Get("Monthly", false);



            notificationManager = DependencyService.Get<INotificationManager>();

        }



        private void SaveLocalNotification()
        {
            var date = (SelectedDate.Date.Month.ToString("00") + "-" + SelectedDate.Date.Day.ToString("00") + "-" + SelectedDate.Date.Year.ToString());
            var time = Convert.ToDateTime(SelectedTime.ToString()).ToString("HH:mm");
            var dateTime = date + " " + time;
            var selectedDateTime = DateTime.ParseExact(dateTime, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long ms = (long)(selectedDateTime.ToUniversalTime() - epoch).TotalMilliseconds;

            Global.selectedDate = ms;

            notificationManager.SavedInfo(nQueue, 0, RandomOn, intervalChosen);

        }


        void CancelNotification()
        {
            UserCheck = false;
            PowerCheck = false;
            WarCheck = false;
            MasteryCheck = false;
            FriendsCheck = false;
            HumanCheck = false;

            EverydayToggle = false;
            OtherDayToggle = false;
            WeeklyToggle = false;
            MonthlyToggle = false;

            notificationManager.Cancel();
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
                // https://stackoverflow.com/questions/57839681/how-to-set-alarm-using-datetime-object-for-specific-date-and-time-in-xamarin-and
                //DateTimeOffset dateOffsetValue = DateTimeOffset.Parse(_selectedDate.ToString());
                //Global.selectedDate = dateOffsetValue.ToUnixTimeMilliseconds();
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
                Global.selectedTime = (long)_selectedTime.TotalMilliseconds - (long)DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
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

        Command _cancelCommand;
        public Command CancelCommand
        {
            get
            {
                return _cancelCommand;
            }
            set
            {
                SetProperty(ref _cancelCommand, value);
            }
        }

        private bool showDate;
        public bool ShowDate
        {
            get => showDate;
            set
            {
                SetProperty(ref showDate, value, nameof(ShowDate));
            }
        }

        private bool showSave;
        public bool ShowSave
        {
            get => showSave;
            set
            {
                SetProperty(ref showSave, value, nameof(ShowSave));
            }
        }


        private bool everydayToggle;
        public bool EverydayToggle
        {
            get => everydayToggle;
            set
            {

                SetProperty(ref everydayToggle, value, nameof(EverydayToggle));
                Preferences.Set("Everyday", value);

                if (value)
                {
                    OtherDayToggle = false;
                    WeeklyToggle = false;
                    MonthlyToggle = false;
                    ShowSave = true;

                    // for debugging
                    intervalChosen = 2000;
                    //intervalChosen = 1000 * 60 * 60 * 24;
                }
                else
                {
                    ShowSave = false;
                }
            }
        }

       

        private bool otherDayToggle;
        public bool OtherDayToggle
        {
            get => otherDayToggle;
            set
            {
                SetProperty(ref otherDayToggle, value, nameof(OtherDayToggle));
                Preferences.Set("Otherday", value);

                if (value)
                {
                    EverydayToggle = false;
                    WeeklyToggle = false;
                    MonthlyToggle = false;
                    ShowDate = true;
                    ShowSave = true;

                    intervalChosen = 1000 * 60 * 60 * 24*3;
                }
                else
                {
                    ShowDate = false;
                    ShowSave = false;
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
                Preferences.Set("Weekly", value);
                if (value)
                {
                    EverydayToggle = false;
                    OtherDayToggle = false;
                    MonthlyToggle = false;
                    ShowDate = true;
                    ShowSave = true;
                    intervalChosen = 1000 * 60 * 60 * 24*7;
                }
                else
                {
                    ShowDate = false;
                    ShowSave = false;
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
                Preferences.Set("Monthly", value);
                if (value)
                {
                    EverydayToggle = false;
                    OtherDayToggle = false;
                    WeeklyToggle = false;
                    ShowDate = true;
                    ShowSave = true;
                    intervalChosen = 2419200000;
                }
                else
                {
                    ShowDate = false;
                    ShowSave = false;
                }
            }
        }

        

        private void setNotifTitle()
        {
            // todo gets called by Preference every start up, check to see that index continues on from last point
            // nQueue is empty and has no key to set to global, if statement prevents the index out of range issue
            if(nQueue.Count == 0)
            {
                Global.notifTitle = null;
            }
            else
            {
                Global.notifTitle = nQueue.ElementAt(0).Key;
            }

        }
        
        private bool userCheck;
        public bool UserCheck
        {
            get => userCheck;
            set
            {
                SetProperty(ref userCheck, value, nameof(UserCheck));
                Preferences.Set("User", value);
                if (value)
                {
                    //todo change the number to a dynamic one
                    
                    nQueue.Add("User", Global.userCount);
                    setNotifTitle();
                }
                else
                {
                    nQueue.Remove("User");
                    setNotifTitle();
                }

            }
        }

        private bool powerCheck;
        public bool PowerCheck
        {
            get => powerCheck;
            set
            {
                SetProperty(ref powerCheck, value, nameof(PowerCheck));
                Preferences.Set("Power", value);
                if (value)
                {
                    nQueue.Add("Power", 47);
                    setNotifTitle();
                }
                else
                {
                    nQueue.Remove("Power");
                    setNotifTitle();
                }
            }
        }

        private bool warCheck;
        public bool WarCheck
        {
            get => warCheck;
            set
            {
                SetProperty(ref warCheck, value, nameof(WarCheck));
                Preferences.Set("War", value);
                if (value)
                {
                    nQueue.Add("War", 32);
                    setNotifTitle();
                }
                else
                {
                    nQueue.Remove("War");
                    setNotifTitle();
                }
            }
        }

        private bool masteryCheck;
        public bool MasteryCheck
        {
            get => masteryCheck;
            set
            {
                SetProperty(ref masteryCheck, value, nameof(MasteryCheck));
                Preferences.Set("Mastery", value);
                if (value)
                {
                    nQueue.Add("Mastery", 17);
                    setNotifTitle();
                }
                else
                {
                    nQueue.Remove("Mastery");
                    setNotifTitle();
                }
            }
        }

        private bool friendsCheck;
        public bool FriendsCheck
        {
            get => friendsCheck;
            set
            {
                SetProperty(ref friendsCheck, value, nameof(FriendsCheck));
                Preferences.Set("Friends", value);
                if (value)
                {
                    nQueue.Add("Friends", 29);
                    setNotifTitle();
                }
                else
                {
                    nQueue.Remove("Friends");
                    setNotifTitle();
                }
            }
        }

        private bool humanCheck;
        public bool HumanCheck
        {
            get => humanCheck;
            set
            {
                SetProperty(ref humanCheck, value, nameof(HumanCheck));
                Preferences.Set("Human", value);
                if (value)
                {
                    nQueue.Add("Human", 17);
                    setNotifTitle();
                }
                else
                {
                    nQueue.Remove("Human");
                    setNotifTitle();
                }
            }
        }

        private bool randomOff;
        public bool RandomOff
        {
            get => randomOff;
            set
            {
                SetProperty(ref randomOff, value, nameof(RandomOff));
                Preferences.Set("RandomOff", value);
                if (value)
                {
                    RandomOn = false;
                }
            }
        }

        private bool randomOn;
        public bool RandomOn
        {
            get => randomOn;
            set
            {
                SetProperty(ref randomOn, value, nameof(RandomOn));
                Preferences.Set("RandomOn", value);
                if (value)
                {
                    RandomOff = false;
                }
            }
        }


    }
}