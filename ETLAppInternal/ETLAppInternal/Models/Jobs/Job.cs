using System.Collections.Generic;
using ETLAppInternal.Models.Clients;
using ETLAppInternal.Models.Sql;

namespace ETLAppInternal.Models.Jobs
{
    public class Jobs
    {
        public int JobId { get; set; }
        public Client Client { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public JobStatus Status { get; set; }

        public static JobsTable ToJobsTable(Jobs job)
        {
            return new JobsTable
            {
                Client = job.Client?.Name,
                FacilityAddress = job.FacilityAddress,
                FacilityName = job.FacilityName,
                JobId = job.JobId,
                Status = job.Status?.Status
            };
        }
    }
}
