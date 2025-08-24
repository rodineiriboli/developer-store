using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeveloperStore.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.OwnsOne(u => u.Name, name =>
            {
                name.Property(n => n.FirstName).HasColumnName("FirstName").HasMaxLength(50);
                name.Property(n => n.LastName).HasColumnName("LastName").HasMaxLength(50);
            });

            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
                address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
                address.Property(a => a.Number).HasColumnName("Number");
                address.Property(a => a.ZipCode).HasColumnName("ZipCode").HasMaxLength(20);

                address.OwnsOne(a => a.GeoLocation, geo =>
                {
                    geo.Property(g => g.Lat).HasColumnName("GeoLat").HasMaxLength(50);
                    geo.Property(g => g.Long).HasColumnName("GeoLong").HasMaxLength(50);
                });
            });

            builder.Property(u => u.Phone)
                .HasMaxLength(20);

            builder.Property(u => u.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<UserStatus>())
                .HasMaxLength(20);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<UserRole>())
                .HasMaxLength(20);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Username).IsUnique();

            builder.Ignore(u => u.DomainEvents);
        }
    }
}