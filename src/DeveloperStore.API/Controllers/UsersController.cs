using DeveloperStore.Application.Commands;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllUsersQuery { Page = page, PageSize = pageSize };
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery { UserId = id };
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var query = new GetUserByEmailQuery { Email = email };
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var query = new GetUserByUsernameQuery { Username = username };
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<UserDto>>> GetUsersByStatus(
            string status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUsersByStatusQuery { Status = status, Page = page, PageSize = pageSize };
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<List<UserDto>>> GetUsersByRole(
            string role,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUsersByRoleQuery { Role = role, Page = page, PageSize = pageSize };
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var command = new CreateUserCommand { UserDto = createUserDto };
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            var command = new UpdateUserCommand { UserId = id, UserDto = updateUserDto };
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand { UserId = id };
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        [HttpPatch("{id}/password")]
        public async Task<ActionResult<UserDto>> ChangePassword(Guid id, [FromBody] string newPassword)
        {
            var command = new ChangeUserPasswordCommand { UserId = id, NewPassword = newPassword };
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<UserDto>> DeactivateUser(Guid id)
        {
            var command = new DeactivateUserCommand { UserId = id };
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<UserDto>> ActivateUser(Guid id)
        {
            var command = new ActivateUserCommand { UserId = id };
            var user = await _mediator.Send(command);
            return Ok(user);
        }
    }
}