using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Image)
                .HasMaxLength(500);

            builder.OwnsOne(p => p.Rating, rating =>
            {
                rating.Property(r => r.Rate).HasColumnName("RatingRate").HasColumnType("decimal(3,2)");
                rating.Property(r => r.Count).HasColumnName("RatingCount");
            });

            builder.HasIndex(p => p.Title).IsUnique();
            builder.HasIndex(p => p.Category);

            builder.Ignore(p => p.DomainEvents);
        }
    }
}