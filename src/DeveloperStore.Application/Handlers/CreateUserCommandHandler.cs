using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsByEmailAsync(request.UserDto.Email))
                throw new DomainException("Email já existe");

            if (await _userRepository.ExistsByUsernameAsync(request.UserDto.Username))
                throw new DomainException("Username já existe");

            var name = new Name(request.UserDto.Name.FirstName, request.UserDto.Name.LastName);
            var geoLocation = new GeoLocation(request.UserDto.Address.GeoLocation.Lat, request.UserDto.Address.GeoLocation.Long);
            var address = new Address(
                request.UserDto.Address.City,
                request.UserDto.Address.Street,
                request.UserDto.Address.Number,
                request.UserDto.Address.ZipCode,
                geoLocation);

            var user = new User(
                request.UserDto.Email,
                request.UserDto.Username,
                request.UserDto.Password,
                name,
                address,
                request.UserDto.Phone,
                request.UserDto.Status,
                request.UserDto.Role
            );

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}