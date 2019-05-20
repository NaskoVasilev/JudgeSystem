namespace JudgeSystem.Web.ViewModels.Problem
{
	using Common;
	using Data.Models;
	using JudgeSystem.Services.Mapping;
	using System.ComponentModel.DataAnnotations;

	public class ProblemEditInputModel : IMapTo<Problem>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public bool IsExtraTask { get; set; }
		
		[Range(GlobalConstants.ProblemMinPoints, GlobalConstants.ProblemMaxPoints)]
		public int MaxPoints { get; set; }
	}
}
