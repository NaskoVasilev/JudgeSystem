using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class CourseTestData
    {
        public static Course GetEntity() => new Course
        {
            Id = 1,
            Name = "Programming Basics"
        };
    }
}
