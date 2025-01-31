using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Services.Accounts;
using ProductsShop.Repositories.Accounts;

internal class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<ServiceResultDTO<IEnumerable<ApplicationUserDTO>>> GetAllUsersAsync()
    {
        var result = await _accountRepository.ReadAllAsync();

        return result;
    }

    public async Task<ServiceResultDTO<ApplicationUserDTO>> GetUserAsync(string email)
    {
        var result = await _accountRepository.ReadByEmailAsync(email);

        return result;
    }

    public async Task<ServiceResultDTO<ApplicationUserDTO>> RegisterUserAsync(RegisterDTO registerDTO)
    {
        var result = await _accountRepository.CreateAccountAsync(registerDTO);

        return result;
    }

    public async Task<ServiceResultDTO<ApplicationUserDTO>> LoginUserAsync(LoginDTO loginDTO)
    {
        var user = await _accountRepository.LoginUserAsync(loginDTO);

        return user;
    }

    public async Task<ServiceResultDTO<string>> LogoutUserAsync()
    {
        var result = await _accountRepository.LogoutUserAsync();

        return result;
    }

    public async Task<ServiceResultDTO<ApplicationUserDTO>> UpdateUserAsync(RegisterDTO registerDTO)
    {
        if (registerDTO.Email == null)
        {
            return new ServiceResultDTO<ApplicationUserDTO>
            {
                IsSuccess = false,
                ErrorMessage = "Email is required",
                ErrorCode = "400"
            };
        }

        var result = await _accountRepository.UpdateAccountAsync(registerDTO);

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

    public async Task<ServiceResultDTO<string>> DeleteUserAsync(string email)
    {
        var result = await _accountRepository.RemoveAccountByEmailAsync(email);

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

