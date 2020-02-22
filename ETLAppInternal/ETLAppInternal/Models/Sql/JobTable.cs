using System;
using ETLAppInternal.Models.Clients;
using ETLAppInternal.Models.Jobs;

namespace ETLAppInternal.Models.Sql
{
    public class JobsTable
    {
        public int JobId { get; set; }
        public string Client { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public string Status { get; set; }

        public static Jobs.Jobs ToJob(JobsTable jobsTable)
        {
            try
            {
                return new Jobs.Jobs
                {
                    Client = new Client {Id = 0, Name = jobsTable.Client},
                    FacilityAddress = jobsTable.FacilityAddress,
                    FacilityName = jobsTable.FacilityName,
                    JobId = jobsTable.JobId,
                    Status = new JobStatus {Id = 0, Status = jobsTable.Status}
                };
            }
            catch (Exception ex)
            {
                var x = ex;
                return new Jobs.Jobs();
            }

        }
    }
}