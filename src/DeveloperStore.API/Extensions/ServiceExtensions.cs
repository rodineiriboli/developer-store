using DeveloperStore.Application;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Infrastructure.Data;
using DeveloperStore.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(ApplicationAssembly.Assembly));

            // Correção: Configurar AutoMapper corretamente
            services.AddAutoMapper(typeof(MappingProfile)); // Passar o tipo, não o Assembly

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configurar DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Registrar repositórios
            services.AddScoped<ISaleRepository, SaleRepository>();

            // Registrar Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}