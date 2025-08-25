using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id);
        Task<Product> GetByTitleAsync(string title);
        Task<List<Product>> GetAllAsync(int page, int pageSize);
        Task<List<Product>> GetByCategoryAsync(string category, int page, int pageSize);
        Task<List<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, int page, int pageSize);
        Task<List<string>> GetAllCategoriesAsync();
        Task<bool> ExistsByTitleAsync(string title);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}