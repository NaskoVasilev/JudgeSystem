namespace JudgeSystem.Data.Models
{
	using System.ComponentModel.DataAnnotations;

	using Data.Models.Enums;

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
