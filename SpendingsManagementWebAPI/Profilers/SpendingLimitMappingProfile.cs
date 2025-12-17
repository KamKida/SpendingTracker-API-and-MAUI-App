using AutoMapper;
using SpendingsManagementWebAPI.Dtos.SpendingLimitDtos;
using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI.Profilers
{
    public class SpendingLimitMappingProfile : Profile
    {
        public SpendingLimitMappingProfile()
        {
            CreateMap<AddSpendingLimitDto, SpendingLimit>();

            CreateMap<SpendingLimit, GetSpendingLimitDto>();

            CreateMap<EditSpendingLimitDto, SpendingLimit>()
                .ForMember(l => l.Id, opt => opt.Ignore())
                .ForMember(l => l.UserId, opt => opt.Ignore());

        }
    }
}
