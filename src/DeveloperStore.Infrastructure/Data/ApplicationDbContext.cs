using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Aqui você pode adicionar lógica para publicar eventos de domínio
            // antes de salvar as mudanças
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}