using AutoMapper;
using SpendingsManagementWebAPI.Dtos.PlanedSpendingDtos;
using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI.Profilers
{
    public class PlanedSpendingMappingProfile : Profile
    {
        public PlanedSpendingMappingProfile()
        {
            CreateMap<PlannedSpending, GetPlanedSpeningDto>()
                .ForMember(s => s.GroupName, opt => opt.MapFrom(o => o.Group.Name));

            CreateMap<AddPlanedSpendingDto, PlannedSpending>();

            CreateMap<EditPlanedSpendingDto, PlannedSpending>()
                .ForMember(l => l.Id, opt => opt.Ignore())
                .ForMember(l => l.UserId, opt => opt.Ignore())
                .ForMember(s => s.GroupId, opt => opt.MapFrom((src, dest) =>
                                            src.GroupId != dest.GroupId ? src.GroupId : dest.GroupId
                ))
                .ForMember(s => s.Name, opt => opt.MapFrom((src, dest) =>
                                            src.Name == null || src.Name == dest.Name ? dest.Name : src.Name
                ));
        }
    }
}
