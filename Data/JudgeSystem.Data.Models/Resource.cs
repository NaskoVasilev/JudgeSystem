using JudgeSystem.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Data.Models
{
	public class Resource
	{
		public int Id { get; set; }

		[Required]
		[MinLength(3)]
		public string Name { get; set; }

		public ResourceType ResourceType { get; set; }

		[Required]
		public string Link { get; set; }

		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }
	}
}
