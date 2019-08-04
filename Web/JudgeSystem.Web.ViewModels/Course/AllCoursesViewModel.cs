using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Course
{
    public class AllCoursesViewModel
    {
        public IEnumerable<CourseViewModel> Courses { get; set; }

        public IEnumerable<string> LessonTypes { get; set; }
    }
}
