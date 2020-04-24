using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Services
{
    public interface INotificationService
    {
        void LocalNotification(string title, string body, int index, DateTime notifyTime, int queueIndex, SerializableDictionary<string, int> notificationQueue, long nextRepeat = 3000);
        void Cancel(int id);
    }
}
