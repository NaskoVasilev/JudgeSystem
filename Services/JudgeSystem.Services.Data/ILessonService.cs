using JudgeSystem.Data.Models;
using JudgeSystem.Web.ViewModels.Lesson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
	public interface ILessonService
	{
		IEnumerable<LessonLinkViewModel> LessonsByType(string lessonType);

		Task CreateLesson(LessonInputModel model, IEnumerable<Resource> resources);

		Task<LessonViewModel> GetLessonInfo(int id);
	}
}
