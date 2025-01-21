using Persistance.DTOs;
using Persistance.DTOs.Accounts;

namespace Persistance.Services.Accounts
{
    public interface IAccountService
    {
        Task<ServiceResultDTO<ApplicationUserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<ServiceResultDTO<ApplicationUserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<ServiceResultDTO<string>> LogoutAsync();
        Task<ServiceResultDTO<string>> DeleteUserAsync(string email);
        Task<ServiceResultDTO<IEnumerable<ApplicationUserDTO>>> GetAllUsersAsync();
    }
}
