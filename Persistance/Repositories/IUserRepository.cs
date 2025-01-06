using Persistance.Context;
using Persistance.DTOs;
using Persistance.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories
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
