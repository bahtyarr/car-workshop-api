using CarWorkshopSystem.Infrastructure.Repositories;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.Infrastructure.Services;
using CarWorkshopSystem.Infrastructure.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarWorkshopSystem.Infrastructure
{
    public static class ServicesExtension
    {
        public static void AddRepositoriesScoped(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICarOwnerRepository, CarOwnerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMechanicRepository, MechanicRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IRepairRepository, RepairRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();


            services.AddScoped<IUserService, UserService>();
        }
    }
}