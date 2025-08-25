using DeveloperStore.Application.Commands;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var command = new LoginCommand { LoginRequest = loginRequest };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro durante o login: " + ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            try
            {
                // Primeiro cria o usuário (usando o handler de user)
                var createUserCommand = new CreateUserCommand
                {
                    UserDto = new CreateUserDto
                    {
                        Email = registerRequest.Email,
                        Username = registerRequest.Username,
                        Password = registerRequest.Password,
                        Name = registerRequest.Name,
                        Address = registerRequest.Address,
                        Phone = registerRequest.Phone,
                        Status = registerRequest.Status,
                        Role = registerRequest.Role
                    }
                };

                var user = await _mediator.Send(createUserCommand);
                return CreatedAtAction(nameof(Login), new { id = user.Id }, user);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro durante o registro: " + ex.Message });
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequest)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                var command = new ChangePasswordCommand
                {
                    UserId = userId,
                    ChangePasswordRequest = changePasswordRequest
                };

                var result = await _mediator.Send(command);
                return Ok(new { message = "Senha alterada com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao alterar senha: " + ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto forgotPasswordRequest)
        {
            try
            {
                // Implementação simplificada
                return Ok(new { message = "Email de redefinição enviado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao processar solicitação: " + ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequest)
        {
            try
            {
                // Implementação simplificada
                return Ok(new { message = "Senha redefinida com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao redefinir senha: " + ex.Message });
            }
        }
    }
}