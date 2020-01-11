using System.Collections.Generic;

using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services;

namespace JudgeSystem.Web.Tests.TestData
{
    public class StudentTestData
    {
        public const string StudentActivationKey = "activation key";

        public static Student GetEntity() => new Student
        {
            Id = "student_id",
            Email = "test@test.me",
            FullName = "Test Student",
            SchoolClass = SchoolClassTestData.GetEntity(),
            ActivationKeyHash = new PasswordHashService().HashPassword(StudentActivationKey),
            NumberInCalss = 2
        };

        public static IEnumerable<Student> GetEntities() => new List<Student>()
            {
                new Student
                {
                    Id = "1",
                    Email = "email1",
                    FullName = "name1",
                    SchoolClass = new SchoolClass
                    {
                        Id = 1,
                        ClassNumber = 12,
                        ClassType = SchoolClassType.A
                    }
                },
                new Student
                {
                    Id = "2",
                    Email = "email2",
                    FullName = "name2",
                    SchoolClass = new SchoolClass
                    {
                        Id = 2,
                        ClassNumber = 12,
                        ClassType = SchoolClassType.B
                    }
                },
                new Student
                {
                    Id = "3",
                    Email = "email4",
                    FullName = "name4",
                    SchoolClass = new SchoolClass
                    {
                        Id = 3,
                        ClassNumber = 11,
                        ClassType = SchoolClassType.A
                    }
                },
                new Student
                {
                    Id = "4",
                    Email = "email4",
                    FullName = "name4",
                    SchoolClass = new SchoolClass
                    {
                        Id = 4,
                        ClassNumber = 11,
                        ClassType = SchoolClassType.B
                    }
                }
            };
    }
}
