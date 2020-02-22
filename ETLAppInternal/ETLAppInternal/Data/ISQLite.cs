using SQLite;

namespace ETLAppInternal.Data
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}
