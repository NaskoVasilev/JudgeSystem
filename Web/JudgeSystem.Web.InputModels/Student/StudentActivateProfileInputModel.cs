using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Student
{
	public class StudentActivateProfileInputModel
	{
		[Required]
        [Display(Name = ModelConstants.StudentActivationKeyDisplayName)]
        public string ActivationKey { get; set; }
	}
}
