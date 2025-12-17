using AutoMapper;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Entities;
using SpendingsManagementWebAPI.Services;

namespace SpendingsManagementWebAPI.Profilers
{
    public class FundMappingProfile : Profile
    {
        public FundMappingProfile()
        {

            CreateMap<AddFundDto, AddedFund>()
                .ForMember(a => a.DateOfCreation, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<EditFundDto, AddedFund>()
                    .ForMember(dest => dest.DateOfCreation, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<AddedFund, GetFundDto>();


        }
    }
}
