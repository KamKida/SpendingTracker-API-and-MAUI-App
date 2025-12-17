using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SpendingsManagementWebAPI.Dtos.AuthenticationDtos;
using SpendingsManagementWebAPI.Dtos.UserDtos;
using SpendingsManagementWebAPI.Entities;
using SpendingsManagementWebAPI.Middlewares;
using SpendingsManagementWebAPI.Profilers;
using SpendingsManagementWebAPI.Services;
using SpendingsManagementWebAPI.Validators.FundValidators;
using SpendingsManagementWebAPI.Validators.GroupLimitValidators;
using SpendingsManagementWebAPI.Validators.GroupValidators;
using SpendingsManagementWebAPI.Validators.PlanedSpendingValidators;
using SpendingsManagementWebAPI.Validators.SpendingLimitValiddators;
using SpendingsManagementWebAPI.Validators.SpendingVlidators;
using SpendingsManagementWebAPI.Validators.UserValidators;
using System.Text;

namespace SpendingsManagementWebAPI.Extencisons
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSpendingService(this IServiceCollection services)
        {
            services.AddDbContext<SpendingDbContext, SpendingDbContext>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(GroupMappingProfile).Assembly);

            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<ErrorHandlingMiddleware>();

            //Serwisy
            services.AddTransient<AccountService>();
            services.AddTransient<UserContextService>();
            services.AddTransient<FundService>();
            services.AddTransient<SpendingService>();
            services.AddTransient<SpendingLimitService>();
            services.AddTransient<GroupService>();
            services.AddTransient<PlanedSpendingsService>();
            //Validatory
            services.AddScoped<RegisterUserValidator>();
            services.AddScoped<EditUserValidator>();

            services.AddScoped<AddFundsValidator>();
            services.AddScoped<EditFundsValidator>();

            services.AddScoped<AddSpendingValidator>();
            services.AddScoped<EditSpendingValiator>();

            services.AddScoped<AddSpendingLimitValidator>();
            services.AddScoped<EditSpendingLimitValidator>();

            services.AddScoped<AddGroupValidator>();
            services.AddScoped<EditGroupValidator>();

            services.AddScoped<GroupLimitValidator>();

            services.AddScoped<AddPlanedSpendingValidator>();
            services.AddScoped<EditPlanedSpendingValidator>();

            return services;
        }
    }
}
