using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DeveloperStore.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, ITokenService tokenService,
                          IEmailService emailService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                throw new UnauthorizedAccessException("Usuário e senha são obrigatórios");

            var user = await _userRepository.GetByUsernameAsync(loginRequest.Username);
            if (user == null)
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            if (!user.VerifyPassword(loginRequest.Password))
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            if (user.Status != UserStatus.Active)
                throw new UnauthorizedAccessException("Usuário inativo. Entre em contato com o administrador");

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Name = new NameDto { FirstName = user.Name.FirstName, LastName = user.Name.LastName },
                Status = user.Status,
                Role = user.Role
            };

            var token = _tokenService.GenerateToken(userDto);

            return new LoginResponseDto
            {
                Token = token,
                User = userDto,
                ExpiresAt = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpiresInHours"]))
            };
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request)
        {
            if (string.IsNullOrEmpty(request.CurrentPassword) || string.IsNullOrEmpty(request.NewPassword))
                throw new ArgumentException("Senha atual e nova senha são obrigatórias");

            if (request.NewPassword.Length < 6)
                throw new ArgumentException("Nova senha deve ter pelo menos 6 caracteres");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado");

            if (!user.VerifyPassword(request.CurrentPassword))
                throw new UnauthorizedAccessException("Senha atual incorreta");

            user.ChangePassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado");

            // Gera um token simples (em produção, use um método mais seguro)
            var token = Guid.NewGuid().ToString();

            return token;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
                throw new ArgumentException("Token e nova senha são obrigatórios");

            if (request.NewPassword.Length < 6)
                throw new ArgumentException("Nova senha deve ter pelo menos 6 caracteres");

            // Implementação simplificada - em produção, validaria o token
            var user = await _userRepository.GetByEmailAsync("email@exemplo.com");
            if (user == null)
                throw new KeyNotFoundException("Token inválido ou expirado");

            user.ChangePassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public string GenerateJwtToken(UserDto user)
        {
            return _tokenService.GenerateToken(user);
        }
    }
}