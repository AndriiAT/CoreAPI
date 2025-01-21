using Microsoft.AspNetCore.Identity;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Models;

namespace ProductsShop.Repositories.Accounts
{
    internal class AccountRepository : IAccountRepository
    {
        public AccountRepository() { }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> IsEmailRegistered(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public bool IsDisposableEmail(string email)
        {
            var disposableDomains = new List<string> { "mailinator.com", "tempmail.com" };
            var emailDomain = email.Split('@')[1];
            return disposableDomains.Contains(emailDomain);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ServiceResultDTO<IEnumerable<ApplicationUserDTO>>> GetAllAsync()
        {
            var users = await Task.Run(() => _userManager.Users.ToList());
            var userDtos = users.Select(user => new ApplicationUserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfCreation = user.DateOfCreation,
                LastModifiedDate = user.LastModifiedDate,
                RoleName = user.RoleName,
                Address = user.Address
            }).ToList();

            return new ServiceResultDTO<IEnumerable<ApplicationUserDTO>>
            {
                IsSuccess = true,
                Data = userDtos
            };
        }

        public async Task<ServiceResultDTO<ApplicationUserDTO>> CreateAsync(RegisterDTO registerDTO)
        {
            if (await IsEmailRegistered(registerDTO.Email))
            {
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Email is already registered.",
                    ErrorCode = "EmailAlreadyRegistered"
                };
            }

            //var roles = await _userManager.GetRolesAsync();
            //if (!roles.Contains(registerDTO.RoleName))
            //{
            //    return new ServiceResultDTO<ApplicationUserDTO>
            //    {
            //        IsSuccess = false,
            //        ErrorMessage = "Role does not exist.",
            //        ErrorCode = "RoleNotFound"
            //    };
            //}

            var user = new ApplicationUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                DateOfCreation = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                RoleName = registerDTO.RoleName,
                Address = registerDTO.Address
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)),
                    ErrorCode = "UserCreationFailed"
                };
            }

            await _userManager.AddToRoleAsync(user, registerDTO.RoleName);

            var userDto = new ApplicationUserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfCreation = user.DateOfCreation,
                LastModifiedDate = user.LastModifiedDate,
                RoleName = user.RoleName,
                Address = user.Address
            };

            return new ServiceResultDTO<ApplicationUserDTO>
            {
                IsSuccess = true,
                Data = userDto
            };
        }

        public async Task<ServiceResultDTO<ApplicationUserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid email or password.",
                    ErrorCode = "InvalidCredentials"
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid email or password.",
                    ErrorCode = "InvalidCredentials"
                };
            }

            var userDto = new ApplicationUserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfCreation = user.DateOfCreation,
                LastModifiedDate = user.LastModifiedDate,
                RoleName = user.RoleName,
                Address = user.Address
            };

            return new ServiceResultDTO<ApplicationUserDTO>
            {
                IsSuccess = true,
                Data = userDto
            };
        }

        public async Task<ServiceResultDTO<string>> RemoveAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ServiceResultDTO<string>
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found.",
                    ErrorCode = "UserNotFound"
                };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ServiceResultDTO<string>
                {
                    IsSuccess = false,
                    ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)),
                    ErrorCode = "UserDeletionFailed"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var roleRemovalTasks = roles.Select(role => _userManager.RemoveFromRoleAsync(user, role));
            var roleRemovalResults = await Task.WhenAll(roleRemovalTasks);

            if (roleRemovalResults.Any(result => !result.Succeeded))
            {
                return new ServiceResultDTO<string>
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to remove user from roles.",
                    ErrorCode = "RoleRemovalFailed"
                };
            }

            return new ServiceResultDTO<string>
            {
                IsSuccess = true,
                Data = "User deleted successfully."
            };
        }

        public async Task<ServiceResultDTO<string>> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new ServiceResultDTO<string>
                {
                    IsSuccess = true,
                    Data = "User logged out successfully."
                };
            }
            catch (Exception ex)
            {
                return new ServiceResultDTO<string>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "LogoutFailed"
                };
            }
        }
    }
}
