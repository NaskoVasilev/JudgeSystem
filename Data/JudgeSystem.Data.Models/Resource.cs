using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Data.Models
{
	public class Resource
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public ResourceType ResourceType { get; set; }

		public string Link { get; set; }

		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }
	}
}
