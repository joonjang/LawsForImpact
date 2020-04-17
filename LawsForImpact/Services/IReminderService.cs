using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Services
{
    public interface IReminderService
    {
        void Remind(DateTime dateTime, string title, string message);
    }
}
