namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Web.InputModels.Student;
	using JudgeSystem.Web.ViewModels.Student;

	public interface IStudentService
	{
		Task<Student> Create(Student student);

		Task<Student> GetStudentProfileByActivationKey(string activationKey);

		Task SetStudentProfileAsActivated(Student student);

		Task<StudentProfileViewModel> GetStudentInfo(string studentId);

		IEnumerable<StudentProfileViewModel> SearchStudentsByClass(int? classNumber, SchoolClassType? classType);

		Task<T> GetById<T>(string id);

		Task<Student> UpdateAsync(StudentEditInputModel model);

		Task DeleteAsync(Student student);

		Task<SchoolClass> GetStudentClassAsync(string id);

		Task<Student> GetById(string id);
	}
}
