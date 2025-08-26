using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using MediatR;
using System.Threading.Channels;

namespace DeveloperStore.Application.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var passwordChanged = await _authService.ChangePasswordAsync(request.UserId, request.ChangePasswordRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return passwordChanged;
        }
    }
}