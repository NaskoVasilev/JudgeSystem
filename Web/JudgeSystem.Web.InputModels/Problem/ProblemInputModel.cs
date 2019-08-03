namespace JudgeSystem.Web.InputModels.Problem
{
	using System.ComponentModel.DataAnnotations;

    using JudgeSystem.Data.Models.Enums;
	using Common;
	using Data.Models;
    using Services.Mapping;

	public class ProblemInputModel : IMapTo<Problem>
	{
        public ProblemInputModel()
        {
            this.AllowedMemoryInMegaBytes = GlobalConstants.DefaultAllowedMemoryInMegaBytes;
            this.AllowedTimeInMilliseconds = GlobalConstants.DefaultAllowedTimeInMilliseconds;
            this.MaxPoints = GlobalConstants.DefaultMaxPoints;
        }

        [Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public bool IsExtraTask { get; set; }
		
		[Range(GlobalConstants.ProblemMinPoints, GlobalConstants.ProblemMaxPoints)]
		public int MaxPoints { get; set; }

        [Range(GlobalConstants.MinAllowedTimeInMilliseconds, GlobalConstants.MaxAllowedTimeInMilliseconds)]
        [Display(Name = "Allowed time in miliseconds")]
        public int AllowedTimeInMilliseconds { get; set; }

        [Range(GlobalConstants.MinAllowedMemoryInMegaBytes, GlobalConstants.MaxAllowedMemoryInMegaBytes)]
        [Display(Name = "Allowed memory in MB")]
        public double AllowedMemoryInMegaBytes { get; set; }

        public int LessonId { get; set; }

        public SubmissionType SubmissionType { get; set; }
    }
}
