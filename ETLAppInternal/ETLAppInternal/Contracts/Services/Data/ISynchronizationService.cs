using System.Threading.Tasks;

namespace ETLAppInternal.Contracts.Services.Data
{
    public interface ISynchronizationService
    {
        Task SynchronizeData();
    }
}
