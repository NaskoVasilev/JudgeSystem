using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.ViewModels.Course;

namespace JudgeSystem.Services.Data
{
    public interface ICourseService
	{
		Task Add(CourseInputModel course);

		IEnumerable<CourseViewModel> All();

		string GetName(int courseId);

		TDestination GetById<TDestination>(int courseId);

		Task Updade(CourseEditModel model);

		Task<CourseViewModel> Delete(int id);
	}
}
