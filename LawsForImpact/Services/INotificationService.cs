using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Services
{
    public interface INotificationService
    {
        void LocalNotification(int id, DateTime notifyTime, int queueIndex, SerializableDictionary<string, int> notificationQueue, long nextRepeat = 3000);
        void Cancel(int id);
    }
}
