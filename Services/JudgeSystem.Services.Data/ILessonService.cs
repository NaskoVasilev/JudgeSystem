using JudgeSystem.Web.ViewModels.Lesson;
using System;
using System.Collections.Generic;
using System.Text;

namespace JudgeSystem.Services.Data
{
	public interface ILessonService
	{
		IEnumerable<LessonLinkViewModel> LessonsByType(string lessonType); 
	}
}
