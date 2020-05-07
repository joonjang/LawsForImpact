using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Services
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        int ScheduleNotification(string title, string message);

        void ReceiveNotification(string title, string message);
        void RepeatAlarmSet();
        void SavedInfo(SerializableDictionary<string, int> pickedQueue, int queueIndex, int index, bool randomTog, int repeatInterval);

        void Cancel();
    }
}
