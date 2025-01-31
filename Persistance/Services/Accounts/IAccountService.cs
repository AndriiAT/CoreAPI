using Persistance.DTOs;
using Persistance.DTOs.Accounts;

namespace Persistance.Services.Accounts
{
    public interface IAccountService
    {
        Task<ServiceResultDTO<ApplicationUserDTO>> RegisterUserAsync(RegisterDTO registerDTO);
        Task<ServiceResultDTO<ApplicationUserDTO>> LoginUserAsync(LoginDTO loginDTO);
        Task<ServiceResultDTO<string>> LogoutUserAsync();
        Task<ServiceResultDTO<ApplicationUserDTO>> UpdateUserAsync(RegisterDTO userDTO);
        Task<ServiceResultDTO<string>> DeleteUserAsync(string email);
        Task<ServiceResultDTO<ApplicationUserDTO>> GetUserAsync(string email);
        Task<ServiceResultDTO<IEnumerable<ApplicationUserDTO>>> GetAllUsersAsync();
    }
}
