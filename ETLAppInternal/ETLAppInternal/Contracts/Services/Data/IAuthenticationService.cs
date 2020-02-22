using System.Threading.Tasks;
using ETLAppInternal.Models.Users;

namespace ETLAppInternal.Contracts.Services.Data
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Authenticate(string userName, string password);

        bool IsUserAuthenticated();
    }
}
