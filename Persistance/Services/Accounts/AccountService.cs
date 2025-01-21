using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Models;
using Persistance.Services.Accounts;
using ProductsShop.Repositories.Accounts;
using System.Runtime.CompilerServices;

internal class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<ServiceResultDTO<IEnumerable<ApplicationUserDTO>>> GetAllUsersAsync()
    {
        var result = await _accountRepository.GetAllAsync();

        if (!result.IsSuccess)
        {
            return new ServiceResultDTO<IEnumerable<ApplicationUserDTO>>
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage,
                ErrorCode = result.ErrorCode
            };
        }

        return new ServiceResultDTO<IEnumerable<ApplicationUserDTO>>
        {
            IsSuccess = true,
            Data = result.Data
        };
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

        var result = await _accountRepository.CreateAsync(registerDTO);

        if (!result.IsSuccess)
        {
            return new ServiceResultDTO<ApplicationUserDTO>
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage,
                ErrorCode = result.ErrorCode
            };
        }

        return new ServiceResultDTO<ApplicationUserDTO>
        {
            IsSuccess = true,
            Data = result.Data
        };
    }

    public async Task<ServiceResultDTO<ApplicationUserDTO>> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _accountRepository.LoginAsync(loginDTO);
        if (user != null)
        {
            return user;
        }

        return new ServiceResultDTO<ApplicationUserDTO>
        {
            IsSuccess = false,
            ErrorMessage = "Invalid login attempt"
        };
    }

    public async Task<ServiceResultDTO<string>> LogoutAsync()
    {
        var result = await _accountRepository.LogoutAsync();

        if (!result.IsSuccess)
        {
            return new ServiceResultDTO<string>
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage,
                ErrorCode = result.ErrorCode
            };
        }

        return new ServiceResultDTO<string>
        {
            IsSuccess = true,
            Data = result.Data
        };
    }

    public async Task<ServiceResultDTO<string>> DeleteUserAsync(string email)
    {
        var result = await _accountRepository.RemoveAsync(email);

        if (!result.IsSuccess)
        {
            return new ServiceResultDTO<string>
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage,
                ErrorCode = result.ErrorCode
            };
        }

        return new ServiceResultDTO<string>
        {
            IsSuccess = true,
            Data = "User deleted successfully"
        };
    }
}

