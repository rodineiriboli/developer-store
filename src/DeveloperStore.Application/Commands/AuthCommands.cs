using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Commands
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public LoginRequestDto LoginRequest { get; set; }
    }

    public class RegisterCommand : IRequest<UserDto>
    {
        public RegisterRequestDto RegisterRequest { get; set; }
    }

    public class ChangePasswordCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public ChangePasswordRequestDto ChangePasswordRequest { get; set; }
    }

    public class ForgotPasswordCommand : IRequest<bool>
    {
        public ForgotPasswordRequestDto ForgotPasswordRequest { get; set; }
    }

    public class ResetPasswordCommand : IRequest<bool>
    {
        public ResetPasswordRequestDto ResetPasswordRequest { get; set; }
    }
}