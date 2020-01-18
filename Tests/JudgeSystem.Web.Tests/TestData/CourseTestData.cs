using System;
using System.Collections.Generic;
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

        internal static IEnumerable<Course> GenerateCourses()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new Course
                {
                    Name = "name" + i,
                    Id = i + 1
                };
            }
        }
    }
}
