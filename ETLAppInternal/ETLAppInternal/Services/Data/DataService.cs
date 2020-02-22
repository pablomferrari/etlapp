using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
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
    public class DataService : BaseService, IDataService
    {
        private readonly IGenericRepository _genericRepository;

        public DataService(IGenericRepository genericRepository,
            IBlobCache cache = null) : base(cache)
        {
            _genericRepository = genericRepository;
        }


        [Obsolete]
       public async Task<IEnumerable<Jobs>> EtcJobs()
        {
            var jobsFromCache =
                  await GetFromCache<List<Jobs>>(CacheNameConstants.EtcJobs);

            if (jobsFromCache != null)//loaded from cache
            {
                return jobsFromCache;
            }
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.EtcJobs
            };
            var jobs = await _genericRepository.GetAsync<List<Jobs>>(builder.ToString());
            try
            {
                await Cache.InsertObject(CacheNameConstants.EtcJobs, jobs);
                var materials = new List<Materials>(); // jobs.SelectMany(x => x.Materials).ToList();
                await Cache.InsertObject(CacheNameConstants.Materials, materials);
                var samples = materials.SelectMany(x => x.Samples).ToList();
                await Cache.InsertObject(CacheNameConstants.Samples, samples);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error retrieving jobs: " + e.Message);
                Crashes.TrackError(e);
                throw;
            }
            return jobs;
        }
        [Obsolete]
        public async Task<IEnumerable<Materials>> Materials(int id, bool forceRefresh = false)
        {
            var materialsFromCache = await GetFromCache<List<Materials>>(CacheNameConstants.Materials);
            if (materialsFromCache != null && !forceRefresh)//loaded from cache
            {
                return materialsFromCache.Where(x => x.JobId == id);
            }
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.Materials
            };
            var serverMaterials = await _genericRepository.GetAsync<List<Materials>>(builder.ToString());
            try
            {
                await Cache.InsertObject(CacheNameConstants.Materials, serverMaterials);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error retrieving materials: " + e.Message);
                Crashes.TrackError(e);
            }
            return serverMaterials;
        }
        [Obsolete]
        public async Task<IEnumerable<Samples>> Samples(int materialId, bool forceRefresh = false)
        {
            var samplesFromCache = await GetFromCache<List<Samples>>(CacheNameConstants.Samples);
            if (samplesFromCache != null && !forceRefresh)//loaded from cache
            {
                return samplesFromCache.Where(x => x.MaterialId == materialId);
            }
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.Samples
            };
            var serverSamples = await _genericRepository.GetAsync<List<Samples>>(builder.ToString());
            
            try
            {
                await Cache.InsertObject(CacheNameConstants.Samples, serverSamples);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error retrieving samples: " + e.Message);
                Crashes.TrackError(e);
            }
            return serverSamples;
        }

        public async Task UpdateMaterial(Materials material)
        {
            try
            {
                var materials = await GetFromCache<List<Materials>>(CacheNameConstants.Materials);
                var updatedMaterial = materials.FirstOrDefault(x =>
                    x.JobId == material.JobId && x.ClientMaterialId == material.ClientMaterialId);
                var index = materials.IndexOf(updatedMaterial);
                if (index > 0)
                {
                    materials[index] = material;
                    await Cache.InsertObject(CacheNameConstants.Materials, materials);
                }
               
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating material: " + e.Message);
                Crashes.TrackError(e);
            }

        }

        public async Task InsertMaterial(Materials material)
        {
            material.CanViewSamples = true;
            var materials = await GetFromCache<List<Materials>>(CacheNameConstants.Materials);
            materials.Add(material);
            await Cache.InsertObject(CacheNameConstants.Materials, materials);
        }

        public async Task UpdateSample(Samples sample)
        {
            try
            {
                var samples = await GetFromCache<List<Samples>>(CacheNameConstants.Samples);
                var updatedSample = samples.FirstOrDefault(x =>
                    x.JobId == sample.JobId && x.ClientSampleId == sample.ClientSampleId);
                var index = samples.IndexOf(updatedSample);
                if (index > 0)
                {
                    samples[index] = sample;
                    await Cache.InsertObject(CacheNameConstants.Samples, samples);
                }
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating sample: " + e.Message);
                Crashes.TrackError(e);
            }
        }

        public async Task InsertSample(Samples sample)
        {
            var samples = await GetFromCache<List<Samples>>(CacheNameConstants.Samples);
            samples.Add(sample);
            await Cache.InsertObject(CacheNameConstants.Samples, samples);
        }

        public async Task PostMaterials()
        {
            var materials = await GetFromCache<List<Materials>>(CacheNameConstants.Materials);
            var newMaterials = materials.Where(x => x.IsLocal && x.Id < 0).ToList();
            
            if (!newMaterials.Any(x => x.IsLocal))//loaded from cache
            {
                return;
            }
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PostMaterials
            };
            try
            {
                await _genericRepository.PostAsync(builder.ToString(), newMaterials);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting materials: " + e.Message);
                Crashes.TrackError(e);
                throw;
            }
        }

        public async Task PutMaterials()
        {
            var materialsFromCache =
                await GetFromCache<List<Materials>>(CacheNameConstants.Materials);


            if (materialsFromCache == null || !materialsFromCache.Any(x => x.IsLocal))//loaded from cache
            {
                return;
            }

            // var putValues = materialsFromCache.Where(x => x.IsLocal).ToList();
            var putValues = materialsFromCache.Where(x => x.IsLocal && x.Id > 0).ToList();
            if (!putValues.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PutMaterials
            };
            try
            {
                await _genericRepository.PutAsync(builder.ToString(), putValues);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating materials: " + e.Message);
                Crashes.TrackError(e);
            }

        }

        public async Task PostSamples()
        {
            var samplesFromCache =
                await GetFromCache<List<Materials>>(CacheNameConstants.Samples);

            if (samplesFromCache == null || !samplesFromCache.Any(x => x.IsLocal))//loaded from cache
            {
                return;
            }

            var postValues = samplesFromCache.Where(x => x.Id < 0).ToList();
            if (!postValues.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PostSamples
            };
            try
            {
                await _genericRepository.PostAsync(builder.ToString(), postValues);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting samples: " + e.Message);
                Crashes.TrackError(e);
            }
        }

        public async Task PutSamples()
        {
            var samplesFromCache =
                await GetFromCache<List<Materials>>(CacheNameConstants.Samples);


            if (samplesFromCache == null || !samplesFromCache.Any(x => x.IsLocal))//loaded from cache
            {
                return;
            }

            var putValues = samplesFromCache.Where(x => x.IsLocal && x.Id > 0).ToList();
            if (!putValues.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PutMaterials
            };
            try
            {
                await _genericRepository.PutAsync(builder.ToString(), putValues);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating samples: " + e.Message);
                Crashes.TrackError(e);
            }
        }

        public async Task ClearCache()
        {
            await BlobCache.LocalMachine.InvalidateAll();
            await BlobCache.LocalMachine.Vacuum();
        }

        public async Task DeliverSamples(DeliveryRequest deliveryRequest)
        {
            var deliveries = await GetFromCache<List<DeliveryRequest>>(CacheNameConstants.DeliveryRequests) 
                ?? new List<DeliveryRequest>();

            if (deliveries.All(x => x.JobId != deliveryRequest.JobId))
            {
                deliveries.Add(deliveryRequest);
                await Cache.InsertObject(CacheNameConstants.DeliveryRequests, deliveries);
            }
            else
            {
                var updatedDelivery = deliveries.FirstOrDefault(x => x.JobId == deliveryRequest.JobId);
                var index = deliveries.IndexOf(updatedDelivery);
                deliveries[index] = deliveryRequest;
                await Cache.InsertObject(CacheNameConstants.Samples, deliveries);
            }
            
        }

        public async Task PostDeliveries()
        {
            var deliveriesFromCache =
                await GetFromCache<List<DeliveryRequest>>(CacheNameConstants.DeliveryRequests);

            if (deliveriesFromCache == null)//loaded from cache
            {
                return;
            }

            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PostDeliveries
            };
            try
            {
                await _genericRepository.PostAsync(builder.ToString(), deliveriesFromCache);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error delivering samples: " + e.Message);
                Crashes.TrackError(e);
            }
        }

        public void AddAll<T>(T values, string key)
        {
            Cache.InsertObject(key, values, TimeSpan.FromDays(15));
        }

        public async Task<IEnumerable<Materials>> GetMaterialsAsync()
        {
            return await GetFromCache<List<Materials>>(CacheNameConstants.NewMaterials);
        }

        public async Task<IEnumerable<Samples>> GetSamplesAsync()
        {
            return await GetFromCache<List<Samples>>(CacheNameConstants.NewSamples);
        }

        public IEnumerable<string> GetKeys()
        {
            var allKeys = Cache.GetAllKeys().Select(x => x).Wait();
            var keys = new List<string>(allKeys.Where(x => x.StartsWith("ETCSyncData_")));
            return keys.Select(x => x.Replace("ETCSyncData_", string.Empty)).OrderByDescending(x => x);
        }

        public async Task<SurveyData> GetSurveyDataAsync(string key)
        {
            return await GetFromCache<SurveyData>("ETCSyncData_" + key);
        }
    }
}
