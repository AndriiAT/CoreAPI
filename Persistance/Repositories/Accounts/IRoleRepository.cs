using Persistance.DTOs.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistance.Repositories.Accounts
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
