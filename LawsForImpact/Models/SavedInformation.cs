using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Models
{
    public class SavedInformation
    {
        public SerializableDictionary<string, int> QueueOfSaved { get; set; }
        public int QueueIndex { get; set; }
        public int Index { get; set; }
        public bool RandomToggle { get; set; }
        public int RepeatInterval { get; set; }
    }
}
