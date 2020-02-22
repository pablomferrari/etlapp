using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Sql;
using Microsoft.AppCenter.Crashes;

namespace ETLAppInternal.Services.Data
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly ISqlLiteService _dataService;
        private readonly IApiService _apiService;
        private readonly ISettingsService _settingsService;
        private readonly IDataService _cacheService;

        public SynchronizationService(
            ISqlLiteService dataService, 
            IApiService apiService, 
            ISettingsService settingsService, 
            IDataService cacheService)
        {
            _dataService = dataService;
            _apiService = apiService;
            _settingsService = settingsService;
            _cacheService = cacheService;
        }

        public async Task SynchronizeData()
        {
            var postMaterials = (await _dataService.GetPostMaterialsAsync()).ToList();
            var putMaterials = (await _dataService.GetPutMaterialsAsync()).ToList();
            var allPostSamples = (await _dataService.GetPostSamplesAsync()).ToList();
            var putSamples = (await _dataService.GetPutSamplesAsync()).ToList();
            var deliveries = (await _dataService.GetDeliveriesAsync()).ToList();
            var materialSamples = postMaterials.SelectMany(x => x.Samples);
            var postSamples = allPostSamples.Where(all => !materialSamples.Select(s => s.Id).Contains(all.Id)).ToList();
            var syncData = new SurveyData
            {
                NewMaterials = postMaterials,
                UpdatedMaterials = putMaterials,
                NewSamples = postSamples,
                UpdatedSamples = putSamples,
                Deliveries = deliveries
            };

            if (syncData.NewMaterials.Any() ||
                syncData.UpdatedSamples.Any() ||
                syncData.NewSamples.Any() ||
                syncData.UpdatedMaterials.Any() ||
                syncData.Deliveries.Any())
            {
                _cacheService.AddAll(syncData, CacheNameConstants.SyncData());
                await _apiService.PostData(syncData).ConfigureAwait(false);
            }
            
            try
            {
                var syncJobTask = SyncJobsAsync();
                var syncMaterials = SyncMaterialsAsync();
                var syncSamplesTask = SyncSamplesAsync();
                var syncMappingTask = SyncMappingsAsync();
                await Task.WhenAll(syncJobTask, syncMaterials, syncSamplesTask, syncMappingTask);
                _dataService.DropAndRecreateDeliveries();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }

        public async Task SyncMaterialsAsync()
        {
            try
            {
                var materials = await _apiService.GetMaterialsAsync();
                var materialsTableList = materials.ToMaterialsTable().ToList();
                try
                {
                    _dataService.DropAndRecreateMaterials();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


                var result = await _dataService.InsertAllAsync<MaterialsTable>(materialsTableList);
                var lastMaterialId = await _dataService.GetLastMaterialIdAsync();
                _settingsService.AddItem(Constants.DatabaseConstants.LastMaterialId, lastMaterialId.ToString());
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }

        public async Task SyncJobsAsync()
        {
            try
            {
                var jobs = await _apiService.GetJobsAsync();
                var jobTableList = jobs.Select(Jobs.ToJobsTable).ToList();
                _dataService.DropAndRecreateJobs();
                await _dataService.InsertAllAsync<JobsTable>(jobTableList);
                var lastJobId = await _dataService.GetLastJobIdAsync();
                _settingsService.AddItem(Constants.DatabaseConstants.LastJobId, lastJobId.ToString());
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }

        public async Task SyncSamplesAsync()
        {
            try
            {
                var samples = await _apiService.GetSamplesAsync();
                var samplesTableList = samples.ToSamplesTable().ToList();
                try
                {
                    _dataService.DropAndRecreateSamples();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                await _dataService.InsertAllAsync<SamplesTable>(samplesTableList);
                var lastSampleId = await _dataService.GetLastSampleIdAsync();
                _settingsService.AddItem(Constants.DatabaseConstants.LastSampleId, lastSampleId.ToString());
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }

        public async Task SyncMappingsAsync()
        {
            try
            {
                var mappings = await _apiService.GetMappingsAsync();
                _dataService.DropAndRecreateMappings();
                var mappingsList = mappings as Mapping[] ?? mappings.ToArray();
                var mappingsTableList = mappingsList.ToMappingsTable();
                var added = await _dataService.InsertAllAsync<MappingsTable>(mappingsTableList.ToList());
                _settingsService.AddItem(Constants.DatabaseConstants.MappingsRan, added.ToString());
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }
    }
}
