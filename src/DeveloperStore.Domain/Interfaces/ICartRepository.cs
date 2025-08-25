using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetByIdAsync(Guid id);
        Task<Cart> GetByUserIdAsync(Guid userId);
        Task<List<Cart>> GetAllAsync(int page, int pageSize);
        Task<List<Cart>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int page, int pageSize);
        Task<bool> ExistsForUserAsync(Guid userId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Cart cart);
    }
}