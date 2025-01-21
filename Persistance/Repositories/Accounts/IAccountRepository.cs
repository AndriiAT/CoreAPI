using Persistance.DTOs;
using Persistance.DTOs.Accounts;

namespace ProductsShop.Repositories.Accounts
{
    public interface IAccountRepository
    {
        Task<ServiceResultDTO<IEnumerable<ApplicationUserDTO>>> GetAllAsync();
        Task<ServiceResultDTO<ApplicationUserDTO>> CreateAsync(RegisterDTO registerDTO);
        Task<ServiceResultDTO<ApplicationUserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<ServiceResultDTO<string>> RemoveAsync(string email);
        Task<ServiceResultDTO<string>> LogoutAsync();
    }
}
