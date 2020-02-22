using System.Collections.Generic;
using System.Threading.Tasks;
using ETLAppInternal.Models.Materials;

namespace ETLAppInternal.Contracts.Services.Data
{
    public interface IMappingsService
    {
        Task<IEnumerable<Mapping>> GetMappings(bool force = false);
    }
}
