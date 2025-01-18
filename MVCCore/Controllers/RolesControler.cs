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
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("all-roles")]
        public async Task<ActionResult<IEnumerable<ApplicationRoleDTO>>> GetRoles()
        {
            var roles = await _roleRepository.ReadAllAsync();
            return Ok(roles);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateRole([FromBody] ApplicationRoleDTO newRole)
        {
            if (newRole == null || string.IsNullOrEmpty(newRole.Name))
            {
                return BadRequest("Role details cannot be empty");
            }

            var createdRole = await _roleRepository.CreateAsync(newRole);
            return Ok(createdRole);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Role ID cannot be empty");
            }

            await _roleRepository.DeleteAsync(id);
            return Ok($"Role with ID {id} deleted successfully");
        }
    }
}
