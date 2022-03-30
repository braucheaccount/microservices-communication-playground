using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Contracts.v1.Requests;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var data = await _userRepository.ListUsersAsync();

            return Ok(data);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
        {
            var data = await _userRepository.FindUserByIdAsync(id);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest)
        {
            var newUser = new User
            {
                Username = createUserRequest.Username,
                Purchases = new()
            };

            var data = await _userRepository.CreateUserAsync(newUser);

            return Ok(data);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            await _userRepository.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
