using System.Collections.Generic;

using JudgeSystem.Web.ViewModels.Lesson;

namespace JudgeSystem.Web.ViewModels.Course
{
    public class CourseLessonsViewModel
    {
        public int CourseId { get; set; }

        public IEnumerable<LessonLinkViewModel> Lessons { get; set; }

        public string Name { get; set; }

        public string LessonType { get; set; }
    }
}
