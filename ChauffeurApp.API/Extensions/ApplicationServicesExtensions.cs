using ChauffeurApp.Application.Services.IServices;
using ChauffeurApp.Application.Services;
using ChauffeurApp.DataAccess.Repositories.IRepositories;
using ChauffeurApp.DataAccess.Repositories;
using FluentValidation;
using ChauffeurApp.Core.Entities;
using ChauffeurApp.Shared.Validation;
using ChauffeurApp.Shared.Services;

namespace ChauffeurApp.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IValidator<Brand>, BrandValidator>();
            services.AddScoped<IValidator<ApplicationUser>, UserValidator>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddTransient<IFileManager, FileManager>();

            return services;
        }
    }
}
