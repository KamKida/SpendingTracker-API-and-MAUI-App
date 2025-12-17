using AutoMapper;
using SpendingsManagementWebAPI.Dtos.GroupLimitDtos;
using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI.Profilers
{
    public class GroupLimitMappingProfile : Profile
    {
        public GroupLimitMappingProfile()
        {
            CreateMap<GroupLimitDto, GroupLimit>();
            CreateMap<GroupLimit, GroupLimitDto>();
        }
    }
}
