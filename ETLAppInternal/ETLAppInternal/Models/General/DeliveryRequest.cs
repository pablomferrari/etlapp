using System.Collections;
using System.Collections.Generic;

namespace ETLAppInternal.Models.General
{
    public class DeliveryRequest
    {
        public int JobId { get; set; }
        public int StatusId { get; set; }
        public int TurnAround { get; set; }
        public int EmployeeId { get; set; }
        public string PlmInstructions { get; set; }
    }
}
