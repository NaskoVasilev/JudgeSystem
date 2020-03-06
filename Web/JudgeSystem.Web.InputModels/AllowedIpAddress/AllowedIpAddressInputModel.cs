using System.ComponentModel.DataAnnotations;

using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.AllowedIpAddress
{
    public class AllowedIpAddressInputModel : IMapTo<Data.Models.AllowedIpAddress>
    {
        [Required]
        public string Value { get; set; }
    }
}
