using Persistance.Context;
using Persistance.DTOs;
using Persistance.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories
{
    public interface IRoleRepository
    {
        Task<ApplicationRoleDTO> CreateAsync(ApplicationRoleDTO role);
        Task<ApplicationRoleDTO> ReadAsync(string roleId);
        Task<ApplicationRoleDTO> UpdateAsync(ApplicationRoleDTO role);
        Task DeleteAsync(string roleId);
        Task<IEnumerable<ApplicationRoleDTO>> ReadAllAsync();
    }
}
