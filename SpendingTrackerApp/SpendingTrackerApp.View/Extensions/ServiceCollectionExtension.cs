using SpendingTrackerApp.Domain.HelpModels;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Infrastructure.Services;
using SpendingTrackerApp.ViewModels;
using SpendingTrackerApp.ViewModels.FundCategoryViewModels;
using SpendingTrackerApp.ViewModels.FundViewModels;
using SpendingTrackerApp.ViewModels.LoginViewModels;
using SpendingTrackerApp.ViewModels.SpendingViewModels;

namespace SpendingTrackerApp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<User>();
            services.AddSingleton<BaseHttpService>();
            services.AddSingleton<JsonService>();
            services.AddSingleton<FundsList>();

            //View models
            services.AddScoped<LoginViewModel>();
            services.AddScoped<CreateAccountViewModel>();
            services.AddScoped<ResetAccountPageViewModel>();
            services.AddScoped<LoadingDataPageViewModel>();

			services.AddScoped<MainPageViewModel>();
            services.AddScoped<FundsHistoryPageViewModel>();
            services.AddScoped<EditFundPageViewModel>();
			services.AddScoped<AddFundPageViewModel>();

            services.AddScoped<FundCategoryListViewModel>();
            services.AddScoped<AddFundCategoryPageViewModel>();
            services.AddScoped<EditFundCategoryPageViewModel>();

            services.AddScoped<SpendingHistoryPageViewModel>();
            services.AddScoped<AddSpendingPageViewModel>();
            services.AddScoped<EditSpendingPageViewModel>();


			//Services
			services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFundService, FundService>();
            services.AddScoped<IFundCategoryService, FundCategotuService>();
            services.AddScoped<ISpendingService, SpendingService>();

            //Auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            return services;
        }
    }
}
