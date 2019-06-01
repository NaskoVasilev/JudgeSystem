using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.ViewModels.Lesson
{
	public class LessonPasswordInputModel
	{
		public int Id { get; set; }

		[DataType(DataType.Password)]
		public string LessonPassword { get; set; }
	}
}
