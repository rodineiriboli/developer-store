using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IAuthService _authService;

        public ChangePasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ChangePasswordAsync(request.UserId, request.ChangePasswordRequest);
        }
    }
}