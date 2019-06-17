using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Data.Models
{
	public class UserContest
	{
		[Required]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public int ContestId { get; set; }
		public Contest Contest { get; set; }
	}
}
