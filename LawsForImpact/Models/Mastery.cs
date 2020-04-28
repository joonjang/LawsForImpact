﻿using LawsForImpact.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Models
{
    [Table("Mastery")]
    public class Mastery : IDataTable
    {
        [PrimaryKey]
        public int Law { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
