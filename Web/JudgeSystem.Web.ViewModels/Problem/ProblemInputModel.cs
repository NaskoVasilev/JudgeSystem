namespace JudgeSystem.Web.ViewModels.Problem
{
	using Common;
	using Data.Models;
	using JudgeSystem.Services.Mapping;
	using System.ComponentModel.DataAnnotations;

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
