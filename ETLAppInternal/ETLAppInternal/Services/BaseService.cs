using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using ETLAppInternal.Models.Jobs;

namespace ETLAppInternal.Services
{
    public class BaseService
    {
        protected IBlobCache Cache;

        public BaseService(IBlobCache cache)
        {
            Cache = cache ?? BlobCache.LocalMachine;
        }

        public async Task<T> GetFromCache<T>(string cacheName)
        {
            try
            {
                T t = await Cache.GetObject<T>(cacheName);
                return t;
            }
            catch (KeyNotFoundException)
            {
                return default(T);
            }
        }

        public async Task InvalidateCache()
        {
            await Cache.InvalidateAllObjects<Jobs>();
        }
    }
}
