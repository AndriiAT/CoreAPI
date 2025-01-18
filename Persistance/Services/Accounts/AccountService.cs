using Microsoft.AspNetCore.Identity;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Models;
using Persistance.Services.Accounts;
using System.Threading.Tasks;

internal class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AccountService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ServiceResultDTO<ApplicationUserDTO>> RegisterAsync(RegisterDTO registerDTO)
    {
        var user = new ApplicationUser
        {
            UserName = registerDTO.Email,
            Email = registerDTO.Email,
            FirstName = registerDTO.FirstName,
            LastName = registerDTO.LastName
        };

        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(registerDTO.RoleId))
            {
                var role = await _roleManager.FindByIdAsync(registerDTO.RoleId);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }

            return new ServiceResultDTO<ApplicationUserDTO>
            {
                IsSuccess = true,
                Data = new ApplicationUserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    DateOfCreation = user.DateOfCreation,
                }
            };
        }

        return new ServiceResultDTO<ApplicationUserDTO>
        {
            IsSuccess = false,
            ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description))
        };
    }

    public async Task<ServiceResultDTO<string>> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, loginDTO.Password))
        {
            return new ServiceResultDTO<string>
            {
                IsSuccess = true,
                Data = "Login successful"
            };
        }

        return new ServiceResultDTO<string>
        {
            IsSuccess = false,
            ErrorMessage = "Invalid login attempt"
        };
    }
}

