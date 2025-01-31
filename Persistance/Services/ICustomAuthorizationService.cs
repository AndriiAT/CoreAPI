using Persistance.DTOs.Accounts;

namespace Persistance.Services
{
    public interface ICustomAuthorizationService
    {
        Task<ApplicationUserDTO> GetAuthorizedUserAsync();
    }
}
