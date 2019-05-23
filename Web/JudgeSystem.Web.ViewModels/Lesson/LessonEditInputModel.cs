namespace JudgeSystem.Web.ViewModels.Lesson
{
	using Common;
	using Data.Models.Enums;
	using Services.Mapping;
	using Data.Models;
	using System.ComponentModel.DataAnnotations;

	public class LessonEditInputModel : IMapTo<Lesson>, IMapFrom<Lesson>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public LessonType Type { get; set; }

		public bool IsLocked { get; set; }
	}
}
