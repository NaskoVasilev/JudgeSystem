using System.Collections.Generic;

using JudgeSystem.Web.ViewModels.Lesson;

namespace JudgeSystem.Web.ViewModels.Course
{
    public class CourseLessonsViewModel
    {
        public IEnumerable<LessonLinkViewModel> Lessons { get; set; }

        public string Name { get; set; }
    }
}
