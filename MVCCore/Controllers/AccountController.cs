﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models.Accounts;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Services.Accounts;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

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


            var result = await _accountService.RegisterUserAsync(registerDTO);
            if (result.IsSuccess)
            {
                var user = result.Data as ApplicationUserDTO;
                if (user == null)
                {
                    return BadRequest("User data is null");
                }

                var accountViewModel = new AccountViewModel
                {
                    UserId = user.UserId,
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

            ServiceResultDTO<ApplicationUserDTO> result = await _accountService.LoginUserAsync(loginDTO);
            if (result.IsSuccess)
            {
                var user = result.Data as ApplicationUserDTO;
                if (user == null)
                {
                    return Unauthorized("Invalid user data");
                }

                var roleName = user.RoleName ?? "User";
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                var accountViewModel = new AccountViewModel
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreationDate = user.DateOfCreation,
                    Role = roleName,
                    Address = user.Address
                };

                return Ok(accountViewModel);
            }

            return Unauthorized(result.ErrorMessage);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUserAsync();
            // Clear user data from cookies
            if (Request.Cookies.ContainsKey("RoleName"))
            {
                Response.Cookies.Delete("RoleName");
            }

            return Ok("User logged out successfully");
        }

        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest();

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return Ok(claims);
        }

        [HttpGet("login-facebook")]
        public IActionResult LoginWithFacebook()
        {
            var redirectUrl = Url.Action("FacebookResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("facebook-response")]
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest();

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return Ok(claims);
        }
    }
}
