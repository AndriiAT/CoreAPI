using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Models;
using Persistance.Services.Accounts;

namespace ProductsShop.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = true,
                    Data = new ApplicationUserDTO
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
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
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);
            if (result.Succeeded)
            {
                return new ServiceResultDTO<string>
                {
                    IsSuccess = true,
                    Data = "User logged in successfully"
                };
            }

            return new ServiceResultDTO<string>
            {
                IsSuccess = false,
                ErrorMessage = "Invalid login attempt"
            };
        }
    }
}
