using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public CreateUserDto UserDto { get; set; }
    }

    public class UpdateUserCommand : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
        public UpdateUserDto UserDto { get; set; }
    }

    public class DeleteUserCommand : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
    }

    public class ChangeUserPasswordCommand : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
    }

    public class DeactivateUserCommand : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
    }

    public class ActivateUserCommand : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
    }
}