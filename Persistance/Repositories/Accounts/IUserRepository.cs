using Persistance.DTOs.Accounts;

namespace Persistance.Repositories.Accounts
{
    public interface IUserRepository
    {
        Task<ApplicationUserDTO> CreateAsync(ApplicationUserDTO user);
        Task<ApplicationUserDTO> ReadAsync(string userId);
        Task<ApplicationUserDTO> UpdateAsync(ApplicationUserDTO user);
        Task DeleteAsync(string userId);
        Task<IEnumerable<ApplicationUserDTO>> ReadAllAsync();
    }
}
