using AutoMapper;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;

namespace SpendingTrackerApp.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User mapping
            CreateMap<User, UserRequest>();
            CreateMap<UserResponse, User>();

            CreateMap<UserRequest, User>();

            //Fund mapping
            CreateMap<Fund, FundRequest>();
            CreateMap<FundResponse, Fund>();

            //Fund cattegory mapping
            CreateMap<FundCategory, FundCategoryRequest>();
            CreateMap<FundCategoryResponse, FundCategory>();

			//Spending mapping
			CreateMap<Spending, SpendingRequest>();
			CreateMap<SpendingResponse, Spending>();

			//Spending cattegory mapping
			CreateMap<SpendingCategory, SpendingCategoryRequest>();
			CreateMap<SpendingCategoryResponse, SpendingCategory>()
			   .ForMember(dest => dest.MonthlyLimitDiffrenceStr,
		opt => opt.MapFrom(src =>
			src.MonthlyLimitDiffrence.HasValue
				? (src.MonthlyLimitDiffrence.Value >= 0
					? $"+ {src.MonthlyLimitDiffrence.Value}"
					: $"- {Math.Abs(src.MonthlyLimitDiffrence.Value)}")
				: null));
		}
    }
}
