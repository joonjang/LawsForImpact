﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LawsForImpact.Views;

namespace LawsForImpact.Droid
{
    [Service]
    public class PeriodicService : Service
    {
        
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // From shared code or in your PCL
            var count = new DebugBackgroundCounter();
            count.CountFunctionOn();

            return StartCommandResult.NotSticky;
        }
    }
}