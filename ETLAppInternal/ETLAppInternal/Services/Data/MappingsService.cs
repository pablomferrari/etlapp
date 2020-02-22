using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Repository;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Models.Materials;

namespace ETLAppInternal.Services.Data
{
    public class MappingsService : BaseService, IMappingsService
    {
        private readonly IGenericRepository _genericRepository;
        public MappingsService(IGenericRepository genericRepository, IBlobCache cache = null) : base(cache)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IEnumerable<Mapping>> GetMappings(bool forceRefresh = false)
        {
            var mappingsFromCache = await GetFromCache<List<Mapping>>(CacheNameConstants.Mappings);
            if (mappingsFromCache != null && !forceRefresh)//loaded from cache
            {
                return mappingsFromCache;
            }
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.Mappings
            };
            try
            {
                var servermappings = await _genericRepository.GetAsync<List<Mapping>>(builder.ToString());
                await Cache.InsertObject(CacheNameConstants.Mappings, servermappings);
                return servermappings;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
