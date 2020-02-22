using SQLite;

namespace ETLAppInternal.Models.Sql
{
    public abstract class BaseTable
    {
        [PrimaryKey]
        public int Id { get; set; }
    }
}