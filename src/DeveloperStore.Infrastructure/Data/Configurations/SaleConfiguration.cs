using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Infrastructure.Data.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SaleNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SaleDate)
                .IsRequired();

            builder.OwnsOne(s => s.Customer, customer =>
            {
                customer.Property(c => c.CustomerId).HasColumnName("CustomerId");
                customer.Property(c => c.Name).HasColumnName("CustomerName").HasMaxLength(100);
                customer.Property(c => c.Email).HasColumnName("CustomerEmail").HasMaxLength(100);
            });

            builder.OwnsOne(s => s.Branch, branch =>
            {
                branch.Property(b => b.BranchId).HasColumnName("BranchId");
                branch.Property(b => b.Name).HasColumnName("BranchName").HasMaxLength(100);
                branch.Property(b => b.Location).HasColumnName("BranchLocation").HasMaxLength(200);
            });

            builder.Property(s => s.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.IsCancelled)
                .IsRequired();

            builder.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey("SaleId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(s => s.DomainEvents);
        }
    }
}