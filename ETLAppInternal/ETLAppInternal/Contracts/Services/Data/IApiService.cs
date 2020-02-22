using System.Collections.Generic;
using System.Threading.Tasks;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;

namespace ETLAppInternal.Contracts.Services.Data
{
    public interface IApiService
    {
        Task<IEnumerable<Jobs>> GetJobsAsync();
        Task<IEnumerable<Materials>> GetMaterialsAsync();
        Task<IEnumerable<Samples>> GetSamplesAsync();
        Task<IEnumerable<Mapping>> GetMappingsAsync();
        Task PostMaterialsAsync(IEnumerable<Materials> materials);
        Task PutMaterialsAsync(IEnumerable<Materials> materials);

        Task PostSamplesAsync(IEnumerable<Samples> samples);
        Task PutSamplesAsync(IEnumerable<Samples> samples);
        Task PostData(SurveyData syncData);
    }
}
