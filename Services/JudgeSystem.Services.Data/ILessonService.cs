using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.InputModels.Lesson;
using JudgeSystem.Web.ViewModels.Lesson;
using JudgeSystem.Web.ViewModels.Search;

namespace JudgeSystem.Services.Data
{
	public interface ILessonService
	{
        IEnumerable<T> GetByCourseId<T>(int courseId)
            where T : IMapFrom<Lesson>;

		IEnumerable<LessonLinkViewModel> CourseLessonsByType(string lessonType, int courseId);

		Task<LessonDto> Create(LessonInputModel model);

		Task<LessonViewModel> GetLessonInfo(int id);

		Task<TDestination> GetById<TDestination>(int id);

		Task<LessonDto> Update(LessonEditInputModel model);

		Task<LessonDto> Delete(int id);

		IEnumerable<ContestLessonDto> GetCourseLesosns(int courseId, LessonType lesosnType);

		IEnumerable<SearchLessonViewModel> SearchByName(string keyword);

        int GetPracticeId(int lessonId);

        int? GetFirstProblemId(int lessonId);

        Task SetPassword(int id, string lessonPassword);

        Task<LessonDto> UpdatePassword(int lessonId, string oldPassword, string newPassword);
    }
}
