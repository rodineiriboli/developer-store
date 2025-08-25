using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Infrastructure.Data.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.Date)
                .IsRequired();

            builder.HasMany(c => c.Products)
                .WithOne()
                .HasForeignKey("CartId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(c => c.DomainEvents);

            builder.HasIndex(c => c.UserId).IsUnique();
        }
    }

    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.ProductId)
                .IsRequired();

            builder.Property(ci => ci.Quantity)
                .IsRequired();
        }
    }
}