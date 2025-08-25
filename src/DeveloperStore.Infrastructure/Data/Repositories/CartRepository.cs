using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetByIdAsync(Guid id)
        {
            return await _context.Carts
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart> GetByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<Cart>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Carts
                .Include(c => c.Products)
                .OrderBy(c => c.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Cart>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            return await _context.Carts
                .Include(c => c.Products)
                .Where(c => c.Date >= startDate && c.Date <= endDate)
                .OrderBy(c => c.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> ExistsForUserAsync(int userId)
        {
            return await _context.Carts
                .AnyAsync(c => c.UserId == userId);
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
        }

        public async Task DeleteAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
        }
    }
}