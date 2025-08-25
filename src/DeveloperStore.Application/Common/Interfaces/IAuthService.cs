using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
        string GenerateJwtToken(UserDto user);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}