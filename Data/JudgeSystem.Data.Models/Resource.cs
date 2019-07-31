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

		[Required]
		public string FilePath { get; set; }

		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }
	}
}
