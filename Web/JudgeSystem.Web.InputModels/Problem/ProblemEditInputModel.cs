namespace JudgeSystem.Web.InputModels.Problem
{
	using System.ComponentModel.DataAnnotations;

	using Common;
	using Data.Models;
    using JudgeSystem.Data.Models.Enums;
    using Services.Mapping;

	public class ProblemEditInputModel : IMapTo<Problem>, IMapFrom<Problem>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public bool IsExtraTask { get; set; }
		
		[Range(GlobalConstants.ProblemMinPoints, GlobalConstants.ProblemMaxPoints)]
		public int MaxPoints { get; set; }

        public SubmissionType SubmissionType { get; set; }
    }
}
