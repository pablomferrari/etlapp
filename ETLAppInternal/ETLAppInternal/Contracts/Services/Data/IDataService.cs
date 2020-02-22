using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;


namespace ETLAppInternal.Contracts.Services.Data
{
    public interface IDataService
    {
        Task<IEnumerable<Jobs>> EtcJobs();
        Task<IEnumerable<Materials>> Materials(int jobId, bool force = false);
        Task<IEnumerable<Samples>> Samples(int materialId, bool force = false);
        Task UpdateMaterial(Materials material);
        Task InsertMaterial(Materials material);
        Task UpdateSample(Samples sample);
        Task InsertSample(Samples sample);

        Task PostMaterials();
        Task PutMaterials();
        Task PostSamples();
        Task PutSamples();
        Task ClearCache();
        Task DeliverSamples(DeliveryRequest deliveryRequest);
        Task PostDeliveries();

        void AddAll<T>(T values, string key);

        Task<IEnumerable<Materials>> GetMaterialsAsync();
        Task<IEnumerable<Samples>> GetSamplesAsync();
        IEnumerable<string> GetKeys();
        Task<SurveyData> GetSurveyDataAsync(string key);
    }
}
