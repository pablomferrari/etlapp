using System.Collections.Generic;
using System.Threading.Tasks;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;
using ETLAppInternal.Models.Sql;

namespace ETLAppInternal.Contracts.Services.Data
{
    public interface ISqlLiteService
    {
        Task<IEnumerable<Jobs>> GetJobsAsync();

        Task<int> InsertOrUpdateAsync<T>(T value) where T : BaseTable;

        Task<int> InsertOrUpdateAllAsync<T>(List<T> value) where T : BaseTable;

        Task<int> InsertAllAsync<T>(List<T> value);

        Task<IEnumerable<Samples>> GetSamplesAsync(int materialId);

        Task<IEnumerable<Materials>> GetMaterialsAsync(int jobId);

        Task<IEnumerable<Mapping>> GetMappingsAsync();
        Task<int> DeleteAsync<T>(int id);
        Task<IEnumerable<Materials>> GetPostMaterialsAsync();
        Task<IEnumerable<Materials>> GetPutMaterialsAsync();
        Task<IEnumerable<Samples>> GetPostSamplesAsync();
        Task<IEnumerable<Samples>> GetPutSamplesAsync();
        Task<IEnumerable<DeliveryRequest>> GetDeliveriesAsync();
        Task<int> GetLastJobIdAsync();
        Task<int> GetLastMaterialIdAsync();
        Task<int> GetLastSampleIdAsync();
        void DropAndRecreateMappings();
        void DropAndRecreateMaterials();
        void DropAndRecreateSamples();
        void DropAndRecreateJobs();
        void DropAndRecreateDeliveries();
        Task AddDelivery(DeliveryRequestTable toDeliveryRequestTable);
    }
}
