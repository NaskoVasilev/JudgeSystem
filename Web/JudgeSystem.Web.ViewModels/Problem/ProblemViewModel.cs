namespace JudgeSystem.Web.ViewModels.Problem
{
	using Services.Mapping;
	using Data.Models;
    using JudgeSystem.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class ProblemViewModel : IMapFrom<Problem>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public bool IsExtraTask { get; set; }

		public int MaxPoints { get; set; }

        [Display(Name = "Allowed time in miliseconds")]
        public int AllowedTimeInMilliseconds { get; set; }

        [Display(Name = "Allowed memory in MB")]
        public double AllowedMemoryInMegaBytes { get; set; }

        public SubmissionType SubmissionType { get; set; }
    }
}
