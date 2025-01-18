using Persistance.DTOs.Accounts;
using Persistance.Repositories.Accounts;

namespace Persistance.Services.Accounts
{
    internal class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<ApplicationRoleDTO>> GetRolesAsync()
        {
            return await _roleRepository.ReadAllAsync();
        }

        public async Task<ApplicationRoleDTO> CreateRoleAsync(ApplicationRoleDTO newRole)
        {
            return await _roleRepository.CreateAsync(newRole);
        }

        public async Task<ApplicationRoleDTO> UpdateRoleAsync(ApplicationRoleDTO updatedRole)
        {
            return await _roleRepository.UpdateAsync(updatedRole);
        }

        public async Task DeleteRoleAsync(string id)
        {
            await _roleRepository.DeleteAsync(id);
        }
    }
}