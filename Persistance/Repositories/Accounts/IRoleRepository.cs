using Persistance.DTOs;
using Persistance.DTOs.Accounts;

namespace Persistance.Repositories.Accounts
{
    public interface IRoleRepository
    {
        Task<ServiceResultDTO<bool>> IsRoleRegistered(string UserRole);
        Task<ServiceResultDTO<ApplicationRoleDTO>> CreateRoleAsync(ApplicationRoleDTO role);
        Task<ServiceResultDTO<ApplicationRoleDTO>> ReadByIdAsync(string roleId);
        Task<ServiceResultDTO<ApplicationRoleDTO>> UpdateRoleAsync(ApplicationRoleDTO role);
        Task<ServiceResultDTO<string>> DeleteRoleAsync(string roleId);
        Task<ServiceResultDTO<IEnumerable<ApplicationRoleDTO>>> ReadAllRolesAsync();
    }
}
