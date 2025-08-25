using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Data.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> GetByIdAsync(Guid id)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Sale>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Sale>> GetByCustomerAsync(Guid customerId, int page, int pageSize)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .Where(s => s.Customer.CustomerId == customerId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
        }

        public async Task DeleteAsync(Sale sale)
        {
            _context.Sales.Remove(sale);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}