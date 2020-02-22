using SQLite;

namespace ETLAppInternal.Models.Sql
{
    public class DeliveryRequestTable
    {
        [PrimaryKey]
        public int JobId { get; set; }
        public int StatusId { get; set; }
        public int TurnAround { get; set; }
        public int EmployeeId { get; set; }
        public string PlmInstructions { get; set; }
    }
}
