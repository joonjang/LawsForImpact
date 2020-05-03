using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Services
{
    public interface INotificationService
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void ReceiveNotification(string title, string message);
        void LocalNotification(int id, DateTime notifyTime, int queueIndex, SerializableDictionary<string, int> notificationQueue, bool randomToggle = false, long nextRepeat = 3000);
        void Cancel(int id);
       
    }
}
