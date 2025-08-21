using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Infrastructure.Data.Configurations
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(si => si.Id);

            builder.OwnsOne(si => si.Product, product =>
            {
                product.Property(p => p.ProductId).HasColumnName("ProductId");
                product.Property(p => p.Name).HasColumnName("ProductName").HasMaxLength(100);
                product.Property(p => p.Description).HasColumnName("ProductDescription").HasMaxLength(500);
            });

            builder.Property(si => si.Quantity)
                .IsRequired();

            builder.Property(si => si.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(si => si.Discount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}