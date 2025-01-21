using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Repositories;
using Persistance.Repositories.Accounts;
using Persistance.Services.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("all-users")]
        public async Task<ActionResult<IEnumerable<ApplicationUserDTO>>> GetUsers()
        {
            var users = await _accountService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterDTO newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.FirstName) || string.IsNullOrEmpty(newUser.LastName))
            {
                return BadRequest("User details cannot be empty");
            }

            var result = await _accountService.RegisterAsync(newUser);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be empty");
            }

            var result = await _accountService.DeleteUserAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok($"User with ID {id} deleted successfully");
        }
    }
}
