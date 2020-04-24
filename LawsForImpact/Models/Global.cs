using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Models
{
    public class Global
    {
        public static int count = -1;
        public static int whichElement = -1;
        public static int whichNotifIndex = 0;
        public static string selectedTitle;
        public static string selectedDescription;
        public static string notifTitle;
        public static string notifCurrentTitle;
        public static int notifCurrentIndex;
        public static string notifDescription;
        public static string notifFullDescrip;
        public static SerializableDictionary<string, int> notifQueue = new SerializableDictionary<string, int>();


    }
}
