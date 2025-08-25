﻿using AutoMapper;
using DeveloperStore.Application.Queries;
using DeveloperStore.Domain.Interfaces;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(request.Page, request.PageSize);
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}