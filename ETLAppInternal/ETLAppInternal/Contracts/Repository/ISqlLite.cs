
using SQLite;

namespace ETLAppInternal.Contracts.Repository
{
    public interface ISqlLite
    {
        SQLiteAsyncConnection GetConnectionAsync();
    }
}
