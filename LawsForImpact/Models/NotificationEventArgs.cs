using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Models
{
    public class NotificationEventArgs : EventArgs
    {
        public string Table { get; set; }
        public int Index { get; set; }
    }
}
