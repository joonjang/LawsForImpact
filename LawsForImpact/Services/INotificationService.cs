using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Services
{
    public interface INotificationService
    {
        void LocalNotification(string title, string body, int id, DateTime notifyTime);
        void Cancel(int id);
    }
}
