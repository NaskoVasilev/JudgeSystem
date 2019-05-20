using JudgeSystem.Data.Models;
using JudgeSystem.Web.ViewModels.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
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
