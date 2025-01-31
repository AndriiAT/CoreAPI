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
            var result = await _roleRepository.ReadAllRolesAsync();
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage);
            }
            return result.Data;
        }

        public async Task<ApplicationRoleDTO> CreateRoleAsync(ApplicationRoleDTO newRole)
        {
            var result = await _roleRepository.CreateRoleAsync(newRole);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage);
            }
            return result.Data;
        }

        public async Task<ApplicationRoleDTO> UpdateRoleAsync(ApplicationRoleDTO updatedRole)
        {
            var result = await _roleRepository.UpdateRoleAsync(updatedRole);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage);
            }
            return result.Data;
        }

        public async Task DeleteRoleAsync(string id)
        {
            var result = await _roleRepository.DeleteRoleAsync(id);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage);
            }
        }
    }
}