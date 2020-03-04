using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.AllowedIpAddress
{
    public class AllowedIpAddressInputModel : IMapTo<Data.Models.AllowedIpAddress>
    {
        public string Value { get; set; }
    }
}
