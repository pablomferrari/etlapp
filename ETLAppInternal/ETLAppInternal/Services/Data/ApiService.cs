using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Repository;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace ETLAppInternal.Services.Data
{
    public class ApiService : IApiService
    {
        private readonly IGenericRepository _genericRepository;
        public ApiService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IEnumerable<Jobs>> GetJobsAsync()
        {
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.Jobs
            };
            return await _genericRepository.GetAsync<List<Jobs>>(builder.ToString());
        }

        public async Task<IEnumerable<Materials>> GetMaterialsAsync()
        {
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.MaterialsV3
            };
            return await _genericRepository.GetAsync<List<Materials>>(builder.ToString());
        }

        public async Task<IEnumerable<Samples>> GetSamplesAsync()
        {
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.SamplesV3
            };
            return await _genericRepository.GetAsync<List<Samples>>(builder.ToString());
        }

        public async Task<IEnumerable<Mapping>> GetMappingsAsync()
        {
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.MappingsV3
            };
            return await _genericRepository.GetAsync<List<Mapping>>(builder.ToString());
        }

        public async Task PutMaterialsAsync(IEnumerable<Materials> materials)
        {
            var list = materials.ToList();
            if (!list.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PutMaterials
            };
            try
            {
                await _genericRepository.PutAsync(builder.ToString(), list);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting materials: " + e.Message);
                Crashes.TrackError(e);
                throw;
            }
        }

        public async Task PostSamplesAsync(IEnumerable<Samples> samples)
        {
            var list = samples.ToList();
            if (!list.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PostSamples
            };
            try
            {
                await _genericRepository.PostAsync(builder.ToString(), list);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting samples: " + e.Message);
                Crashes.TrackError(e);
            }
        }

        public async Task PutSamplesAsync(IEnumerable<Samples> samples)
        {
            var list = samples.ToList();
            if (!list.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PutSamples
            };
            try
            {
                await _genericRepository.PutAsync(builder.ToString(), list);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating samples: " + e.Message);
                Crashes.TrackError(e);
            }
        }
        
        public async Task PostMaterialsAsync(IEnumerable<Materials> materials)
        {
            var list = materials.ToList();
            if (!list.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PostMaterials
            };
            try
            {
                await _genericRepository.PostAsync(builder.ToString(), materials);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting materials: " + e.Message);
                Crashes.TrackError(e);
                throw;
            }
        }

        public async Task PostData(SurveyData syncData)
        {
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PostData
            };
            try
            {
                await _genericRepository.PostAsync(builder.ToString(), syncData);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting data: " + e.Message);
                Crashes.TrackError(e);
                throw;
            }
        }
    }
}
