using JudgeSystem.Services.Mapping;
using AutoMapper;

namespace JudgeSystem.Web.ViewModels.AllowedIpAddress
{
    public class AllowedIpAddressViewModel : IMapFrom<Data.Models.AllowedIpAddress>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public void CreateMappings(IProfileExpression configuration) => 
            configuration.CreateMap<Data.Models.AllowedIpAddressContest, AllowedIpAddressViewModel>()
                .ForMember(m => m.Value, y => y.MapFrom(s => s.AllowedIpAddress.Value));
    }
}
