namespace JudgeSystem.Web.InputModels.Student
{
	using System.ComponentModel.DataAnnotations;

	public class StudentActivateProfileInputModel
	{
		[Required]
		public string ActivationKey { get; set; }
	}
}
