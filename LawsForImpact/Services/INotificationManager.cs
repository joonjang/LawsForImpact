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

        void ReceiveNotification(string tableKey, int indexKey);
        void RepeatAlarmSet();
        void SavedInfo(SerializableDictionary<string, int> pickedQueue, int queueIndex, bool randomTog, long repeatInterval);

        void Cancel();
    }
}
