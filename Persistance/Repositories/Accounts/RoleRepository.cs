using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DTOs.Accounts;
using Persistance.Models;

namespace Persistance.Repositories.Accounts
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApplicationRoleDTO> CreateAsync(ApplicationRoleDTO role)
        {
            var existingRoleById = await _roleManager.FindByIdAsync(role.Id);
            var existingRoleByName = await _roleManager.FindByNameAsync(role.Name);

            if (existingRoleById != null || existingRoleByName != null)
            {
                return null; // Role with the same Id or Name already exists
            }

            var identityRole = new ApplicationRole
            {
                Id = string.IsNullOrEmpty(role.Id) ? Guid.NewGuid().ToString() : role.Id,
                Name = role.Name,
                Description = role.Description,
                CreationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
            var result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return new ApplicationRoleDTO
                {
                    Id = identityRole.Id,
                    Name = identityRole.Name,
                    Description = role.Description,
                    CreationDate = role.CreationDate,
                    ModifiedDate = role.ModifiedDate
                };
            }

            return null;
        }

        public async Task<ApplicationRoleDTO> ReadAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return null;
            }

            return new ApplicationRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.NormalizedName,
                CreationDate = role.CreationDate,
                ModifiedDate = role.ModifiedDate
            };
        }

        public async Task<ApplicationRoleDTO> UpdateAsync(ApplicationRoleDTO role)
        {
            var identityRole = await _roleManager.FindByIdAsync(role.Id);
            if (identityRole == null)
            {
                return null;
            }

            identityRole.Name = role.Name;
            identityRole.Description = role.Description;
            identityRole.ModifiedDate = DateTime.UtcNow;

            var result = await _roleManager.UpdateAsync(identityRole);

            if (result.Succeeded)
            {
                return new ApplicationRoleDTO
                {
                    Id = identityRole.Id,
                    Name = identityRole.Name,
                    Description = identityRole.Description,
                    CreationDate = identityRole.CreationDate,
                    ModifiedDate = identityRole.ModifiedDate
                };
            }

            return null;
        }

        public async Task DeleteAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }

        public async Task<IEnumerable<ApplicationRoleDTO>> ReadAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(role => new ApplicationRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.NormalizedName,
                CreationDate = role.CreationDate
            });
        }
    }
}
