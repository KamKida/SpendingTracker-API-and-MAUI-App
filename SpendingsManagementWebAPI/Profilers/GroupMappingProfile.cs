using AutoMapper;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Dtos.GroupDtos;
using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI.Profilers
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<AddGroupDto, Group>();
            CreateMap<Group, GetGroupDto>();

            CreateMap<EditGroupDto, Group>()
                .ForMember(g => g.Id, opt => opt.Ignore())
                .ForMember(g => g.UserId, opt => opt.Ignore())
                .ForMember(s => s.Name, opt => opt.MapFrom((src, dest) =>
                                               src.Name == null || src.Name == dest.Name ? dest.Name : src.Name))
                .ForMember(s => s.Description, opt => opt.MapFrom((src, dest) =>
                                               src.Description == null || src.Description == dest.Description ? dest.Description : src.Description));
                


        }
    }
}
