using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LawsForImpact.Droid.DebugService
{
    [Service]
    public class CounterService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			var count = intent.GetStringExtra("DebugCount");

			Task.Run(() => {
				
			});

			return StartCommandResult.Sticky;
		}
	}
}