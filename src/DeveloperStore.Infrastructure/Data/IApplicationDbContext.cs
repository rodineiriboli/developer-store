using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Sale> Sales { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}