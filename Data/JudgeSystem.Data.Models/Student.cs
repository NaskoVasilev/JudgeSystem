using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Models
{
    public class Student : BaseModel<string>
	{
        [Required]
        [MaxLength(ModelConstants.StudentFullNameMaxLength)]
		public string FullName { get; set; }

		[Required]
		public string ActivationKeyHash { get; set; }

		[Required]
        [MaxLength(ModelConstants.StudentEmailMaxLength)]
		public string Email { get; set; }

		public int NumberInCalss { get; set; }

		public bool IsActivated { get; set; } = false;

		public int SchoolClassId { get; set; }
		public SchoolClass SchoolClass { get; set; }

		public ApplicationUser User { get; set; }
	}
}
