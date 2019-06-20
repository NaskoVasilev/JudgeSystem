namespace JudgeSystem.Services.Data
{
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;

	public interface IStudentService
	{
		Task<Student> Create(Student student);

		Task<Student> GetStudentProfileByActivationKey(string activationKey);

		Task SetStudentProfileAsActivated(Student student);
	}
}
