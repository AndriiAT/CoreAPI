using Persistance.DTOs;
using Persistance.DTOs.Accounts;

namespace ProductsShop.Repositories.Accounts
{
    public interface IAccountRepository
    {
        Task<ServiceResultDTO<IEnumerable<ApplicationUserDTO>>> ReadAllAsync();
        Task<ServiceResultDTO<ApplicationUserDTO>> ReadByEmailAsync(string email);
        Task<ServiceResultDTO<ApplicationUserDTO>> CreateAccountAsync(RegisterDTO registerDTO);
        Task<ServiceResultDTO<ApplicationUserDTO>> LoginUserAsync(LoginDTO loginDTO);
        Task<ServiceResultDTO<ApplicationUserDTO>> UpdateAccountAsync(RegisterDTO existingUser);
        Task<ServiceResultDTO<string>> RemoveAccountByEmailAsync(string email);
        Task<ServiceResultDTO<string>> LogoutUserAsync();
    }
}
