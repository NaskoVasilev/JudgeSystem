namespace JudgeSystem.Web.InputModels.Lesson
{
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Enums;
    using Services.Mapping;
    using Data.Models;
    using Common;

    public class LessonInputModel : IMapTo<Lesson>
	{
		[Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public int CourseId { get; set; }

		[Display(Name = "Lesson Password")]
		[DataType(DataType.Password)]
		public string LessonPassword { get; set; }

		[Required]
		public LessonType Type { get; set; }
	}
}
