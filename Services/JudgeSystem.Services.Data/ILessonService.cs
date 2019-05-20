using JudgeSystem.Data.Models;
using JudgeSystem.Web.ViewModels.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
	public interface ILessonService
	{
		IEnumerable<LessonLinkViewModel> CourseLessonsByType(string lessonType, int courseId);

		Task<Lesson> CreateLesson(LessonInputModel model, IEnumerable<Resource> resources);

		Task<LessonViewModel> GetLessonInfo(int id);

		Task<Lesson> GetById(int id);
	}
}
