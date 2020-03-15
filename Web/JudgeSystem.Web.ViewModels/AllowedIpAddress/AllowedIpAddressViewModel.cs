using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.AllowedIpAddress
{
    public class AllowedIpAddressViewModel : IMapFrom<Data.Models.AllowedIpAddress>
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }
}
