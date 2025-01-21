using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models.Accounts;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountBuildingModel registerModel)
        {
            if (registerModel == null || string.IsNullOrEmpty(registerModel.Mail) || string.IsNullOrEmpty(registerModel.Password))
            {
                return BadRequest("Invalid registration details");
            }

            var registerDTO = new RegisterDTO
            {
                Email = registerModel.Mail,
                Password = registerModel.Password,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Address = registerModel.Address,
            };

            // Check if the user is authorized and has the admin role
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                registerDTO.RoleName = registerModel.Role;
            else
                registerDTO.RoleName = "User";


            var result = await _accountService.RegisterAsync(registerDTO);
            if (result.IsSuccess)
            {
                var user = result.Data as ApplicationUserDTO;
                if (user == null)
                {
                    return BadRequest("User data is null");
                }

                var accountViewModel = new AccountViewModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreationDate = user.DateOfCreation,
                    Role = user.RoleName,
                    Address = user.Address
                };

                return Ok(accountViewModel);
            }

            if (result.IsSuccess)
            {
                return Ok("User registered successfully");
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginBuildingModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Invalid login details");
            }

            var loginDTO = new LoginDTO
            {
                Email = loginModel.Email,
                Password = loginModel.Password
            };

            ServiceResultDTO<ApplicationUserDTO> result = await _accountService.LoginAsync(loginDTO);
            if (result.IsSuccess)
            {
                var user = result.Data as ApplicationUserDTO;
                if (user == null)
                {
                    return Unauthorized("Invalid user data");
                }

                var roleName = user.RoleName ?? "User"; // Fetching role name from result if available
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                Response.Cookies.Append("RoleName", roleName, cookieOptions);

                var accountViewModel = new AccountViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreationDate = user.DateOfCreation,
                    Role = roleName
                };

                return Ok(accountViewModel);
            }

            return Unauthorized(result.ErrorMessage);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            // Clear user data from cookies
            if (Request.Cookies.ContainsKey("RoleName"))
            {
                Response.Cookies.Delete("RoleName");
            }

            return Ok("User logged out successfully");
        }
    }
}
