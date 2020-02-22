using System.Collections.Generic;
using System.Threading.Tasks;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;
using ETLAppInternal.Models.Sql;
using static ETLAppInternal.App;

namespace ETLAppInternal.Services.Data
{
    public class SqlLiteService : ISqlLiteService
    {
        public async Task<IEnumerable<Jobs>> GetJobsAsync()
        {
            return await Database.GetJobsAsync().ConfigureAwait(false);
        }


        public async Task<IEnumerable<Mapping>> GetMappingsAsync()
        {
            return await Database.GetMappingsAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<DeliveryRequest>> GetDeliveries()
        {
            return await Database.GetDeliveriesAsync().ConfigureAwait(false);
        }


        public async Task<int> DeleteAsync<T>(int id)
        {
            return await Database.DeleteAsync<T>(id);
        }

        public async Task<IEnumerable<Materials>> GetPostMaterialsAsync()
        {
            return await Database.GetPostMaterialsAsync();
        }

        public async Task<IEnumerable<Materials>> GetPutMaterialsAsync()
        {
            return await Database.GetPutMaterialsAsync();
        }

        public async Task<IEnumerable<Samples>> GetPutSamplesAsync()
        {
            return await Database.GetPutSamplesAsync();
        }

        public async Task<IEnumerable<Samples>> GetPostSamplesAsync()
        {
            return await Database.GetPostSamplesAsync();
        }

        public async Task<IEnumerable<DeliveryRequest>> GetDeliveriesAsync()
        {
            return await Database.GetDeliveriesAsync();
        }

        public async Task<int> GetLastJobIdAsync()
        {
            return await Database.ExecuteScalar("SELECT JobId FROM JobsTable order by JobId desc LIMIT 1 ");
        }

        public async Task<int> GetLastMaterialIdAsync()
        {
            return await Database.ExecuteScalar("SELECT Id FROM MaterialsTable order by Id desc  LIMIT 1 ");
        }

        public async Task<int> GetLastSampleIdAsync()
        {
            return await Database.ExecuteScalar("SELECT Id FROM SamplesTable order by Id desc  LIMIT 1 ");
        }

        public void DropAndRecreateMappings()
        {
            Database.DropAndRecreateMappings();
        }

        public void DropAndRecreateMaterials()
        {
            Database.DropAndRecreateMaterials();
        }

        public void DropAndRecreateSamples()
        {
            Database.DropAndRecreateSamples();
        }

        public void DropAndRecreateJobs()
        {
            Database.DropAndRecreateJobs();
        }

        public void DropAndRecreateDeliveries()
        {
            Database.DropAndRecreateDeliveries();
        }

        public async Task AddDelivery(DeliveryRequestTable delivery)
        {
            await Database.AddDeliveryAsync(delivery);
        }


        public async Task<IEnumerable<Materials>> GetMaterialsAsync(int jobId)
        {
            return await Database.GetMaterialsAsync(jobId);
        }
        
        public async Task<int> InsertOrUpdateAllAsync<T>(List<T> values) where T : BaseTable
        {
            return await Database.InsertOrReplaceAllAsync<T>(values);
        }

        public async Task<int> InsertAllAsync<T>(List<T> values)
        {
            return await Database.InsertAllAsync<T>(values);
        }

        public async Task<IEnumerable<Samples>> GetSamplesAsync(int materialId)
        {
            return await Database.GetSamplesAsync(materialId);
        }

        public async Task<int> InsertOrUpdateAsync<T>(T value) where T : BaseTable
        {
            return await Database.InsertOrReplaceAsync(value);
        }
    }
}
