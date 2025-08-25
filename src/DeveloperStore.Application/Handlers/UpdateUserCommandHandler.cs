using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.UserId} not found");

            // Check if email already exists for another user
            var existingUserWithEmail = await _userRepository.GetByEmailAsync(request.UserDto.Email);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != request.UserId)
                throw new DomainException("Email already exists");

            // Check if username already exists for another user
            var existingUserWithUsername = await _userRepository.GetByUsernameAsync(request.UserDto.Username);
            if (existingUserWithUsername != null && existingUserWithUsername.Id != request.UserId)
                throw new DomainException("Username already exists");

            var name = new Name(request.UserDto.Name.FirstName, request.UserDto.Name.LastName);
            var geoLocation = new GeoLocation(request.UserDto.Address.GeoLocation.Lat, request.UserDto.Address.GeoLocation.Long);
            var address = new Address(
                request.UserDto.Address.City,
                request.UserDto.Address.Street,
                request.UserDto.Address.Number,
                request.UserDto.Address.ZipCode,
                geoLocation);

            user.Update(
                request.UserDto.Email,
                request.UserDto.Username,
                name,
                address,
                request.UserDto.Phone,
                request.UserDto.Status,
                request.UserDto.Role);

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}