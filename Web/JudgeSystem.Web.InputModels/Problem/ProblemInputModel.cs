namespace JudgeSystem.Web.InputModels.Problem
{
	using System.ComponentModel.DataAnnotations;

    using JudgeSystem.Data.Models.Enums;
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

        public SubmissionType SubmissionType { get; set; }
    }
}
