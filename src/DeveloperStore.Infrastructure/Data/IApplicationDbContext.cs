using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Data
{
    public interface IApplicationDbContext
    {
        // DbSets
        DbSet<User> Users { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Sale> Sales { get; set; }
        DbSet<SaleItem> SaleItems { get; set; }
        DbSet<Cart> Carts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}