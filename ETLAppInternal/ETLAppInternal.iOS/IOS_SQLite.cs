using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ETLAppInternal.Constants;
using ETLAppInternal.Data;
using ETLAppInternal.iOS;
using Foundation;
using SQLite;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOS_SQLite))]
namespace ETLAppInternal.iOS
{
    public class IOS_SQLite : ISQLite
    {
        public SQLiteAsyncConnection GetConnection()
        { 
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder  
            string libraryPath = Path.Combine(dbPath, "..", "Library"); // Library folder  
            var path = Path.Combine(libraryPath, DatabaseConstants.DbName);
            return new SQLiteAsyncConnection(path);
        }
    }
}