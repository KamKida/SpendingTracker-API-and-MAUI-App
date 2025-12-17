using Microsoft.AspNetCore.Identity;
using SpendingTracker.Application.ContextServices;
using SpendingTracker.Application.Interfaces.ContextServices;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Application.Services;
using SpendingTracker.Domain.Models;
using SpendingTracker.Infrastructure.Context;
using SpendingTracker.Infrastructure.Middlewares;
using SpendingTracker.Infrastructure.Validators;

namespace SpendingTracker.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //Database
            services.AddDbContext<SpendingTrackerDbContext, SpendingTrackerDbContext>();

            //Password hasher
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            //Middlewares
            services.AddScoped<ErrorHandlingMiddleware>();

            //Validators
            services.AddScoped<UserRequestValidator>();
            services.AddScoped<UserEditRequestValidator>();
           
            //Auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Services
            services.AddScoped<IUserService, UserService>();

            //Context services
            services.AddScoped<IUserContextService,UserContextService>();

            
            return services;
        }
    }
}
