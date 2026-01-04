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

            //Fund mapping
            CreateMap<Fund, FundRequest>();
            CreateMap<FundResponse, Fund>();
            CreateMap<FundRequest, Fund>();
        }
    }
}
