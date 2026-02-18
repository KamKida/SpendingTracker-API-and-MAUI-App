using AutoMapper;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Responses;
using SpendingTracker.Domain.Models;

namespace SpendingTracker.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User mapping
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();

            //Fund mapping
            CreateMap<Fund, FundResponse>();
            CreateMap<FundRequest, Fund>();

			//Fund category mapping
			CreateMap<FundCategory, FundCategoryResponse>();
            CreateMap<FundCategoryRequest, FundCategory>();

            //Spending mapping
            CreateMap<Spending, SpendingReponse>();
            CreateMap<SpendingRequest, Spending>();

			//Spending category mapping
			CreateMap<SpendingCategory, SpendingCategoryResponse>();
			CreateMap<SpendingCategoryRequest, SpendingCategory>();
		}
    }
}
