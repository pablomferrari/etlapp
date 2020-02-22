using System.IO;
using ETLAppInternal.Constants;
using ETLAppInternal.Data;
using ETLAppInternal.Droid;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace ETLAppInternal.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
        }

        #region ISQLite implementation

        public SQLiteAsyncConnection GetConnection()
        {
            var path = Path.Combine(System.Environment.
                GetFolderPath(System.Environment.
                    SpecialFolder.Personal), DatabaseConstants.DbName);

            return new SQLiteAsyncConnection(path);
        }

        #endregion
    }
}