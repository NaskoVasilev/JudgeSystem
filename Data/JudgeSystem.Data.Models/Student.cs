namespace JudgeSystem.Data.Models
{
	using JudgeSystem.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;

    public class Student : BaseModel<string>
	{
		public int FullName { get; set; }

		[Required]
		public string ActivationKeyHash { get; set; }

		[Required]
		public string Email { get; set; }

		public int NumberInCalss { get; set; }

		public int SchoolClassId { get; set; }
		public SchoolClass SchoolClass { get; set; }

		public ApplicationUser User { get; set; }
	}
}
