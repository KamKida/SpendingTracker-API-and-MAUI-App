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
            CreateMap<FundResponse, Fund>()
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.CreationDate.ToString("dd-MM-yyyy HH-mm")));
        }
    }
}
