namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Web.Dtos.Lesson;
	using JudgeSystem.Web.InputModels.Lesson;
	using JudgeSystem.Web.ViewModels.Lesson;
	using JudgeSystem.Web.ViewModels.Search;

	public interface ILessonService
	{
		IEnumerable<LessonLinkViewModel> CourseLessonsByType(string lessonType, int courseId);

		Task<Lesson> CreateLesson(LessonInputModel model, IEnumerable<Resource> resources);

		Task<LessonViewModel> GetLessonInfo(int id);

		Task<Lesson> GetById(int id);

		Task Update(Lesson lesson);

		Task Delete(Lesson lesson);

		IEnumerable<ContestLessonDto> GetCourseLesosns(int courseId, LessonType lesosnType);

		IEnumerable<SearchLessonViewModel> SearchByName(string keyword);
	}
}
