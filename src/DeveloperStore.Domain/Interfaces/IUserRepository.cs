using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<List<User>> GetAllAsync(int page, int pageSize);
        Task<List<User>> GetByStatusAsync(string status, int page, int pageSize);
        Task<List<User>> GetByRoleAsync(string role, int page, int pageSize);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}