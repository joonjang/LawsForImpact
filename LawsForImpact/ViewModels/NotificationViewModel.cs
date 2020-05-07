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
        int notificationNumber = 0;

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



            notificationManager = DependencyService.Get<INotificationManager>();

        }



        private void SaveLocalNotification()
        {

            notificationManager.SavedInfo(nQueue, 0, 0, false, 3000);

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

        void CancelNotification()
        {
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