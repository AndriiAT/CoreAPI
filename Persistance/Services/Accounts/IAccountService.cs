using Persistance.DTOs;
using Persistance.DTOs.Accounts;

namespace Persistance.Services.Accounts
{
    public interface IAccountService
    {
        Task<ServiceResultDTO<ApplicationUserDTO>> RegisterAsync(DTOs.Accounts.RegisterDTO registerDTO);
        Task<ServiceResultDTO<string>> LoginAsync(LoginDTO loginDTO);
    }
}
