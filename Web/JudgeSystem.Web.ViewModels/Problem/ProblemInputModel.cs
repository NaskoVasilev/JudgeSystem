namespace JudgeSystem.Web.ViewModels.Problem
{
	using System.ComponentModel.DataAnnotations;

	using Common;
	using Data.Models;
	using Services.Mapping;

	public class ProblemInputModel : IMapTo<Problem>
	{
		[Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public bool IsExtraTask { get; set; }
		
		[Range(GlobalConstants.ProblemMinPoints, GlobalConstants.ProblemMaxPoints)]
		public int MaxPoints { get; set; }

		public int LessonId { get; set; }
	}
}
