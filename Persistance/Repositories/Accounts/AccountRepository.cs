using Microsoft.AspNetCore.Identity;
using Persistance.Context;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Models;
using Persistance.Repositories.Accounts;
using System.Data;

namespace ProductsShop.Repositories.Accounts
{
    internal class AccountRepository : IAccountRepository
    {

        public AccountRepository() { }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ShopDbContext _context;
        private readonly IRoleRepository _roleRepository;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ShopDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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

        public async Task<ServiceResultDTO<ApplicationUserDTO>> ReadByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found.",
                    ErrorCode = "UserNotFound"
                };
            }

            var userDto = new ApplicationUserDTO
            {
                UserId = user.Id,
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

        public async Task<ServiceResultDTO<IEnumerable<ApplicationUserDTO>>> ReadAllAsync()
        {
            var users = await Task.Run(() => _userManager.Users.ToList());
            IEnumerable<ApplicationUserDTO> userDtos = users.Select(user => new ApplicationUserDTO
            {
                UserId = user.Id,
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

        public async Task<ServiceResultDTO<ApplicationUserDTO>> CreateAccountAsync(RegisterDTO registerDTO)
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

            var user = new ApplicationUser
            {
                AccessFailedCount = 0,
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                DateOfCreation = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                RoleName = registerDTO.RoleName,
                Address = registerDTO.Address
            };

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(registerDTO.RoleName))
            {
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Role does not exist.",
                    ErrorCode = "RoleNotFound"
                };
            }

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
                UserId = user.Id,
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

        public async Task<ServiceResultDTO<ApplicationUserDTO>> LoginUserAsync(LoginDTO loginDTO)
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
                await LogLoginAttemptAsync(loginDTO.Email, true, null);

                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid email or password.",
                    ErrorCode = "InvalidCredentials"
                };
            }

            var userDto = new ApplicationUserDTO
            {
                UserId = user.Id,
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

        private async Task LogLoginAttemptAsync(string email, bool isSuccess, string errorMessage)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var log = new UserLoginLog
            {
                LogId = Guid.NewGuid().ToString(),
                UserId = user?.Id,
                Email = email,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                Timestamp = DateTime.UtcNow
            };
            _context.UserLoginLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ShopDbContext context, IRoleRepository roleRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleRepository = roleRepository;
        }

        public async Task<ServiceResultDTO<ApplicationUserDTO>> UpdateAccountAsync(RegisterDTO existingUser)
        {
            var user = await _userManager.FindByEmailAsync(existingUser.Email);
            if (user == null)
            {
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found.",
                    ErrorCode = "UserNotFound"
                };
            }
            if (existingUser.FirstName != null && existingUser.FirstName != user.FirstName)
            {
                user.FirstName = existingUser.FirstName;
            }
            if (existingUser.LastName != null && existingUser.LastName != user.LastName)
            {
                user.LastName = existingUser.LastName;
            }
            if (existingUser.RoleName != null && existingUser.RoleName != user.RoleName)
            {
                user.RoleName = existingUser.RoleName;
            }
            if (existingUser.Address != null && existingUser.Address != user.Address)
            {
                user.Address = existingUser.Address;
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var roleExists = await _roleRepository.IsRoleRegistered(existingUser.RoleName);
                if (!roleExists.IsSuccess)
                {
                    var newRole = new ApplicationRoleDTO
                    {
                        Name = user.RoleName,
                        Description = $"{user.RoleName} Role",
                    };
                    var roleResult = await _roleRepository.CreateRoleAsync(newRole);
                    if (!roleResult.IsSuccess)
                    {
                        return new ServiceResultDTO<ApplicationUserDTO>
                        {
                            IsSuccess = false,
                            ErrorMessage = "Role creation failed.",
                            ErrorCode = "RoleCreationFailed"
                        };
                    }
                }
                return new ServiceResultDTO<ApplicationUserDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)),
                    ErrorCode = "UserUpdateFailed"
                };
            }
            var userDto = new ApplicationUserDTO
            {
                UserId = user.Id,
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

        public async Task<ServiceResultDTO<string>> RemoveAccountByEmailAsync(string email)
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

        public async Task<ServiceResultDTO<string>> LogoutUserAsync()
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
