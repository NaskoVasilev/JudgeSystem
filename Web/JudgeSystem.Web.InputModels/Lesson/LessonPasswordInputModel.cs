using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.InputModels.Lesson
{
	public class LessonPasswordInputModel
	{
		public int Id { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Lesson Password")]
		public string LessonPassword { get; set; }
	}
}
