using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Infrastructure.Services;
using SpendingTrackerApp.Pages;
using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<User>();
            services.AddSingleton<BaseHttpService>();

            //View models
            services.AddScoped<LoginViewModel>();
            services.AddScoped<CreateAccountViewModel>();
            services.AddScoped<ResetAccountPageViewModel>();

            //Services
            services.AddScoped<IUserService, UserService>();

            //Auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
