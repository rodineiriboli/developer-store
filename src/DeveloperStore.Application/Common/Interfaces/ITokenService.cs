namespace DeveloperStore.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserDto user);
        bool ValidateToken(string token);
        string GetUserIdFromToken(string token);
    }
}