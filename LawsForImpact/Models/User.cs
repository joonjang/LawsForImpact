using LawsForImpact.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Models
{
    [Table("User")]
    public class User : IDataTable
    {
        [PrimaryKey, AutoIncrement]
        public int Law { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        
    }
}
