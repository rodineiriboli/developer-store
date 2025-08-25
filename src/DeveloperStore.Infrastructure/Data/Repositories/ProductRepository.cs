using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetByTitleAsync(string title)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Title == title);
        }

        public async Task<List<Product>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Products
                .OrderBy(p => p.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Product>> GetByCategoryAsync(string category, int page, int pageSize)
        {
            return await _context.Products
                .Where(p => p.Category == category)
                .OrderBy(p => p.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, int page, int pageSize)
        {
            return await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .OrderBy(p => p.Price)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllCategoriesAsync()
        {
            return await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.Products
                .AnyAsync(p => p.Title == title);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}