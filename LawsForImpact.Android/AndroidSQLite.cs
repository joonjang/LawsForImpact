using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LawsForImpact.Droid;
using LawsForImpact.Services;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidSQLite))]
namespace LawsForImpact.Droid
{
    
    public class AndroidSQLite : ISQLite
    {
        // Gets ths data from Assets
        async Task<SQLiteConnection> ISQLite.GetConnection()
        {

            String databaseName = "LawOfPower.db";
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbFile = Path.Combine(docFolder, databaseName); // FILE NAME TO USE WHEN COPIED
            if (!File.Exists(dbFile))
            {
                FileStream writeStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write);
                await Forms.Context.Assets.Open(databaseName).CopyToAsync(writeStream);
            }

            var path = dbFile;
            // Create the connection
            var conn = new SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}