using JudgeSystem.Web.ViewModels.AllowedIpAddress;
using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Contest
{
    public class ContestAllowedIpAddressesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<AllowedIpAddressViewModel> AllowedIpAddresses { get; set; }
    }
}
