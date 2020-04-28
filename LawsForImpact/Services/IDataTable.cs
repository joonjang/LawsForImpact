using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawsForImpact.Services
{
    public interface IDataTable
    {
        [PrimaryKey]
        int Law { get; set; }
         string Title { get; set; }
         string Description { get; set; }
    }
}
