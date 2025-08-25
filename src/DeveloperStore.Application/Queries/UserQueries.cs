using MediatR;

namespace DeveloperStore.Application.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserByEmailQuery : IRequest<UserDto>
    {
        public string Email { get; set; }
    }

    public class GetUserByUsernameQuery : IRequest<UserDto>
    {
        public string Username { get; set; }
    }

    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetUsersByStatusQuery : IRequest<List<UserDto>>
    {
        public string Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetUsersByRoleQuery : IRequest<List<UserDto>>
    {
        public string Role { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}