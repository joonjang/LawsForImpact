using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Services
{
    public interface IDebugNotiServ
    {
        void LocalNotification(string title, string body, int id, DateTime notifyTime);
        void Cancel(int id);
    }
}
