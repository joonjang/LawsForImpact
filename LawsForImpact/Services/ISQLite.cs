using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LawsForImpact.Services
{
    public interface ISQLite
    {
        Task<SQLiteConnection> GetConnection();
    }
}
