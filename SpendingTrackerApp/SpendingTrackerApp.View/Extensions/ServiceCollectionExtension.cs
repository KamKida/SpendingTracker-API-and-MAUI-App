using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Infrastructure.Services;
using SpendingTrackerApp.ViewModels;
using SpendingTrackerApp.ViewModels.FundCategoryViewModels;
using SpendingTrackerApp.ViewModels.FundViewModels;
using SpendingTrackerApp.ViewModels.LoginViewModels;
using SpendingTrackerApp.ViewModels.SpendingCategoryViewModels;
using SpendingTrackerApp.ViewModels.SpendingViewModels;
using SpendingTrackerApp.ViewModels.UserPagesViewModels;

namespace SpendingTrackerApp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<BaseHttpService>();
            services.AddSingleton<JsonService>();
            services.AddSingleton<User>();

            //View models
            services.AddScoped<LoginViewModel>();
            services.AddScoped<CreateAccountViewModel>();
            services.AddScoped<ResetAccountPageViewModel>();

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

            services.AddScoped<SpendingCategoryListPageViewModel>();
            services.AddScoped<AddSpendingCategoryPageViewModel>();
            services.AddScoped<EditSpendingCategoryPageViewModel>();

            services.AddScoped<EditUserPageViewModel>();

			//Services
			services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFundService, FundService>();
            services.AddScoped<IFundCategoryService, FundCategoryService>();
            services.AddScoped<ISpendingService, SpendingService>();
            services.AddScoped<ISpendingCategoryService, SpendingCategoryService>();

            //Auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            return services;
        }
    }
}
