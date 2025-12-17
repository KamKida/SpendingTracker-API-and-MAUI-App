using AutoMapper;
using SpendingsManagementWebAPI.Dtos.SpendingDtos;
using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI.Profilers
{
    public class SpendingMappingProfile : Profile
    {
        public SpendingMappingProfile()
        {
            CreateMap<AddSpendingDto, Spending>();

            CreateMap<EditSpendingDto, Spending>()
                .ForMember(s => s.UserId, opt => opt.Ignore())
                .ForMember(s => s.GroupId, opt => opt.MapFrom((src, dest) =>
                                            src.GroupId != dest.GroupId ? src.GroupId : dest.GroupId
                ));

            CreateMap<Spending, GetSpedingDto>()
                .ForMember(s => s.GroupName, opt => opt.MapFrom(sp => sp.Group.Name != null ? sp.Group.Name : null));
        }
    }
}
