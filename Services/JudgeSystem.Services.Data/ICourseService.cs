namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.ViewModels.Course;

	public interface ICourseService
	{
		Task Add(CourseInputModel course);

		IEnumerable<CourseViewModel> All();

		string GetName(int courseId);

		Task<Course> GetById(int courseId);

		Task Updade(CourseEditModel model);

		Task Delete(Course course);
	}
}
