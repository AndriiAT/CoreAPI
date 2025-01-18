using Persistance.DTOs.Accounts;

namespace Persistance.Services.Accounts
{
    public interface IRoleService
    {
        Task<IEnumerable<ApplicationRoleDTO>> GetRolesAsync();
        Task<ApplicationRoleDTO> CreateRoleAsync(ApplicationRoleDTO newRole);
        Task<ApplicationRoleDTO> UpdateRoleAsync(ApplicationRoleDTO updatedRole);
        Task DeleteRoleAsync(string id);
    }
}
