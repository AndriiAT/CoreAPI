using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DTOs;
using Persistance.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApplicationRoleDTO> CreateAsync(ApplicationRoleDTO roleDTO)
        {
            var role = new ApplicationRole
            {
                Name = roleDTO.Name,
                Description = roleDTO.Description,
                CreatedDate = roleDTO.CreatedDate
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                roleDTO.Id = role.Id;
                return roleDTO;
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
                Description = role.Description,
                CreatedDate = role.CreatedDate
            };
        }

        public async Task<ApplicationRoleDTO> UpdateAsync(ApplicationRoleDTO roleDTO)
        {
            var role = await _roleManager.FindByIdAsync(roleDTO.Id);
            if (role == null)
            {
                return null;
            }

            role.Name = roleDTO.Name;
            role.Description = roleDTO.Description;
            role.CreatedDate = roleDTO.CreatedDate;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return roleDTO;
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
                Description = role.Description,
                CreatedDate = role.CreatedDate
            }).ToList();
        }
    }
}
