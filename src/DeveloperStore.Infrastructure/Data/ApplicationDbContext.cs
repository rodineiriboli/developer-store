using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets para todas as entidades
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignora a classe DomainEventBase para não ser mapeada como entidade
            //modelBuilder.Ignore<CartEvents>();
            modelBuilder.Ignore<DomainEventBase>();
            //modelBuilder.Ignore<ItemCancelledEvent>();
            //modelBuilder.Ignore<ProductEvents>();
            //modelBuilder.Ignore<SaleCancelledEvent>();
            //modelBuilder.Ignore<SaleCreatedEvent>();
            //modelBuilder.Ignore<SaleModifiedEvent>();
            //modelBuilder.Ignore<UserEvents>();
            //CartEvents
            //ItemCancelledEvent
            //ProductEvents
            //SaleCancelledEvent
            //SaleCreatedEvent
            //SaleModifiedEvent
            //UserEvents


            // Aplicar todas as configurações das entidades
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configurações específicas se necessário
            ConfigureModel(modelBuilder);
        }

        private void ConfigureModel(ModelBuilder modelBuilder)
        {
            // Configurações adicionais podem ser adicionadas aqui
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