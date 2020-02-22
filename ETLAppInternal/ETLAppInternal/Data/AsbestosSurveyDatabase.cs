using System;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Sql;
using Xamarin.Forms;

namespace ETLAppInternal.Data
{
    public class AsbestosSurveyDatabase
    {
        readonly SQLiteAsyncConnection database;

        public AsbestosSurveyDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTablesAsync(CreateFlags.None,
                new Type[]
                {
                    new JobsTable().GetType(),
                    new MaterialsTable().GetType(),
                    new SamplesTable().GetType(),
                    new DeliveryRequestTable().GetType(),
                    new MappingsTable().GetType()
                }
            ).Wait();

        }

        public async Task<IEnumerable<Jobs>> GetJobsAsync()
        {
            var jobs = await database.Table<JobsTable>().ToListAsync().ConfigureAwait(false);
            return jobs.Select(JobsTable.ToJob).ToList();
        }
        
        public async Task<int> GetLastJobIdAsync()
        {
            var jobs = await database.Table<JobsTable>().ToListAsync().ConfigureAwait(false);
            var lastJob = jobs.OrderByDescending(x => x.JobId).FirstOrDefault();
            return lastJob?.JobId ?? 0;
        }

        public async Task<int> InsertAllAsync<T>(List<T> jobs)
        {
            return await database.InsertAllAsync(jobs, true);
        }

        public async Task<int> InsertOrReplaceAllAsync<T>(List<T> jobs)
        {
            var updated = 0;
            foreach(var job in jobs)
            {
                await database.InsertOrReplaceAsync(job);
                updated++;
            }
            return updated;
        }

        public async Task<int> DeleteAsync<T>(int value)
        {
            return await database.DeleteAsync<T>(value);
        }

        public async Task<int> InsertOrReplaceAsync<T>(T value) where T : BaseTable
        {
            return await database.InsertOrReplaceAsync(value);
        }

        public async Task<IEnumerable<Materials>> GetMaterialsAsync(int jobId)
        {
            var materialsTable = await database.Table<MaterialsTable>().Where(x => x.JobId == jobId).ToListAsync();
            return materialsTable.ToMaterials();
        }

        public async Task<IEnumerable<Samples>> GetSamplesAsync(int materialId)
        {
            var samplesTable = await database.Table<SamplesTable>().Where(x => x.MaterialId == materialId).ToListAsync();
            return samplesTable.ToSamples();
        }

        public async Task<long> GetLastSampleIdAsync()
        {
            var samples = await database.Table<SamplesTable>().ToListAsync().ConfigureAwait(false);
            var lastSample = samples.OrderByDescending(x => x.Id).FirstOrDefault();
            return  lastSample?.Id ?? 0;
        }

        public async Task<int> GetLastMaterialIdAsync()
        {
            var materials = await database.Table<MaterialsTable>().ToListAsync().ConfigureAwait(false);
            var lastMaterial = materials.OrderByDescending(x => x.Id).FirstOrDefault();
            return lastMaterial?.Id ?? 0;
        }

        public async Task<IEnumerable<Mapping>> GetMappingsAsync()
        {
            var mappings = await database.Table<MappingsTable>().ToListAsync().ConfigureAwait(false);
            return mappings.ToMappingModel();
        }

        public async Task<IEnumerable<Materials>> GetPostMaterialsAsync()
        {
            var result = database.Table<MaterialsTable>().Where(x => x.IsNew).ToListAsync().Result;
            var materials = result.ToMaterials().ToList();
            foreach (var material in materials)
            {
                var st = await database.Table<SamplesTable>().Where(x => x.MaterialId == material.Id).ToListAsync();
                    
                material.Samples = st.ToSamples().ToList();
            }
            return materials;
        }

        public async Task<IEnumerable<Materials>> GetPutMaterialsAsync()
        {
            var result = await database.Table<MaterialsTable>().Where(x => x.IsNew == false && x.IsLocal)
                .ToListAsync()
                .ConfigureAwait(false);
            return result.ToMaterials();
        }

        public async Task<IEnumerable<Samples>> GetPostSamplesAsync()
        {
            var result = await database.Table<SamplesTable>().Where(x => x.IsNew).ToListAsync();
            return result.ToSamples();
        }

        public async Task<IEnumerable<Samples>> GetPutSamplesAsync()
        {
            var result = await database.Table<SamplesTable>().Where(x => !x.IsNew && x.IsLocal).ToListAsync();
            return result.ToSamples();
        }

        public async Task<int> ExecuteScalar(string query)
        {
            return await database.ExecuteScalarAsync<int>(query);
        }

        public void DropAndRecreateJobs()
        {
            database.DropTableAsync<JobsTable>().Wait();
            database.CreateTableAsync<JobsTable>().Wait();
        }

        public void DropAndRecreateMaterials()
        {
            database.DropTableAsync<MaterialsTable>().Wait();
            database.CreateTableAsync<MaterialsTable>().Wait();
        }

        public void DropAndRecreateSamples()
        {
            database.DropTableAsync<SamplesTable>().Wait();
            database.CreateTableAsync<SamplesTable>().Wait();
        }


        public void DropAndRecreateMappings()
        {
            database.DropTableAsync<MappingsTable>().Wait();
            database.CreateTableAsync<MappingsTable>().Wait();
        }

        public async Task AddDeliveryAsync(DeliveryRequestTable delivery)
        {
            await database.InsertOrReplaceAsync(delivery);
        }

        public void DropAndRecreateDeliveries()
        {
            database.DropTableAsync<DeliveryRequestTable>().Wait();
            database.CreateTableAsync<DeliveryRequestTable>().Wait();
        }

        public async Task<IEnumerable<DeliveryRequest>> GetDeliveriesAsync()
        {
            var result = await database.Table<DeliveryRequestTable>().ToListAsync().ConfigureAwait(false);
            return result.ToDeliveriesModel();
        }
    }
}
