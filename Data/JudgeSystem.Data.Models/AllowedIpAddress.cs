using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Models
{
    public class AllowedIpAddress : BaseDeletableModel<int>
    {
        public AllowedIpAddress()
        {
            Contests = new HashSet<AllowedIpAddressContest>();
        }

        [Required]
        public string Value { get; set; }

        public ICollection<AllowedIpAddressContest> Contests { get; set; }
    }
}
