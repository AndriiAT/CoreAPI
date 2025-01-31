using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistance.DTOs.Accounts;
using Persistance.Services;
using Persistance.Services.Accounts;
using System.Security.Claims;

public class CustomAuthorizationService : ICustomAuthorizationService
{
    private readonly HttpContext _httpContext;
    private readonly IAccountService _accountService;
    private readonly ILogger<CustomAuthorizationService> _logger;

    public CustomAuthorizationService(IHttpContextAccessor httpContextAccessor, IAccountService accountService, ILogger<CustomAuthorizationService> logger)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _accountService = accountService;
        _logger = logger;
    }

    public async Task<ApplicationUserDTO> GetAuthorizedUserAsync()
    {
        var userEmailClaim = _httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmailClaim))
        {
            _logger.LogWarning("Unauthorized access attempt with no email claim.");
            throw new UnauthorizedAccessException("User is not authorized.");
        }

        var userResult = await _accountService.GetUserAsync(userEmailClaim);
        if (!userResult.IsSuccess)
        {
            _logger.LogWarning($"Failed to retrieve user with email {userEmailClaim}. Error: {userResult.ErrorMessage}");
            throw new UnauthorizedAccessException("User is not authorized.");
        }

        return userResult.Data;
    }
}
