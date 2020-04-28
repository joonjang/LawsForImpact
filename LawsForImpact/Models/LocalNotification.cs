using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Models
{
    public class LocalNotification
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public int Index { get; set; }

        public int IconId { get; set; }

        public DateTime NotifyTime { get; set; }

        public long RepeatInterval { get; set; }

        public SerializableDictionary<string, int> NotificationQueue { get; set; }

        public int QueueIndex { get; set; }

        public bool RandomToggle { get; set; }
    }
}
