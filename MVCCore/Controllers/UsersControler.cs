using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.DTOs;
using Persistance.DTOs.Accounts;
using Persistance.Repositories;
using Persistance.Repositories.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("all-users")]
        public async Task<ActionResult<IEnumerable<ApplicationUserDTO>>> GetUsers()
        {
            var users = await _userRepository.ReadAllAsync();
            return Ok(users);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateUser([FromBody] ApplicationUserDTO newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.FirstName) || string.IsNullOrEmpty(newUser.LastName))
            {
                return BadRequest("User details cannot be empty");
            }

            var createdUser = await _userRepository.CreateAsync(newUser);
            return Ok(createdUser);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be empty");
            }

            await _userRepository.DeleteAsync(id);
            return Ok($"User with ID {id} deleted successfully");
        }
    }
}
