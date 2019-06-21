namespace JudgeSystem.Services.Data
{
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.ViewModels.Student;

	public interface IStudentService
	{
		Task<Student> Create(Student student);

		Task<Student> GetStudentProfileByActivationKey(string activationKey);

		Task SetStudentProfileAsActivated(Student student);

		Task<StudentProfileViewModel> GetStudentInfo(string studentId);
	}
}
