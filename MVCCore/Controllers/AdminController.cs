using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.DTOs;
using Persistance.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/admin/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
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
