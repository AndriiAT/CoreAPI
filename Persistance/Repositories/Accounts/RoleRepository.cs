using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Models;
using System.Data;

namespace Persistance.Repositories.Accounts
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ServiceResultDTO<bool>> IsRoleRegistered(string UserRole)
        {
            var role = await _roleManager.FindByNameAsync(UserRole);
            return new ServiceResultDTO<bool>
            {
                IsSuccess = role != null,
                Data = role != null
            };
        }

        public async Task<ServiceResultDTO<ApplicationRoleDTO>> CreateRoleAsync(ApplicationRoleDTO role)
        {
            var existingRoleById = await _roleManager.FindByIdAsync(role.RoleId);
            var existingRoleByName = await _roleManager.FindByNameAsync(role.Name);

            if (existingRoleById != null || existingRoleByName != null)
            {
                return new ServiceResultDTO<ApplicationRoleDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Role with the same ProductId or Name already exists"
                };
            }

            var identityRole = new ApplicationRole
            {
                Id = $"{role.Name}-role-id",
                Name = role.Name,
                Description = role.Description,
                CreationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
            var result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return new ServiceResultDTO<ApplicationRoleDTO>
                {
                    IsSuccess = true,
                    Data = new ApplicationRoleDTO
                    {
                        RoleId = identityRole.Id,
                        Name = identityRole.Name,
                        Description = role.Description,
                        CreationDate = role.CreationDate,
                        ModifiedDate = role.ModifiedDate
                    }
                };
            }

            return new ServiceResultDTO<ApplicationRoleDTO>
            {
                IsSuccess = false,
                ErrorMessage = "Failed to create role"
            };
        }

        public async Task<ServiceResultDTO<ApplicationRoleDTO>> ReadByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return new ServiceResultDTO<ApplicationRoleDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Role not found"
                };
            }

            return new ServiceResultDTO<ApplicationRoleDTO>
            {
                IsSuccess = true,
                Data = new ApplicationRoleDTO
                {
                    RoleId = role.Id,
                    Name = role.Name,
                    Description = role.NormalizedName,
                    CreationDate = role.CreationDate,
                    ModifiedDate = role.ModifiedDate
                }
            };
        }

        public async Task<ServiceResultDTO<ApplicationRoleDTO>> UpdateRoleAsync(ApplicationRoleDTO role)
        {
            var identityRole = await _roleManager.FindByIdAsync(role.RoleId);
            if (identityRole == null)
            {
                return new ServiceResultDTO<ApplicationRoleDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Role not found"
                };
            }

            identityRole.Name = role.Name;
            identityRole.Description = role.Description;
            identityRole.ModifiedDate = DateTime.UtcNow;

            var result = await _roleManager.UpdateAsync(identityRole);

            if (result.Succeeded)
            {
                return new ServiceResultDTO<ApplicationRoleDTO>
                {
                    IsSuccess = true,
                    Data = new ApplicationRoleDTO
                    {
                        RoleId = identityRole.Id,
                        Name = identityRole.Name,
                        Description = identityRole.Description,
                        CreationDate = identityRole.CreationDate,
                        ModifiedDate = identityRole.ModifiedDate
                    }
                };
            }

            return new ServiceResultDTO<ApplicationRoleDTO>
            {
                IsSuccess = false,
                ErrorMessage = "Failed to update role"
            };
        }

        public async Task<ServiceResultDTO<string>> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return new ServiceResultDTO<string>
                    {
                        IsSuccess = true,
                        Data = roleId
                    };
                }
                return new ServiceResultDTO<string>
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to delete role"
                };
            }
            return new ServiceResultDTO<string>
            {
                IsSuccess = false,
                ErrorMessage = "Role not found"
            };
        }

        public async Task<ServiceResultDTO<IEnumerable<ApplicationRoleDTO>>> ReadAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return new ServiceResultDTO<IEnumerable<ApplicationRoleDTO>>
            {
                IsSuccess = true,
                Data = roles.Select(role => new ApplicationRoleDTO
                {
                    RoleId = role.Id,
                    Name = role.Name,
                    Description = role.NormalizedName,
                    CreationDate = role.CreationDate,
                    ModifiedDate = role.ModifiedDate
                })
            };
        }
    }
}
