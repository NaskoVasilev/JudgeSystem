using JudgeSystem.Data.Models;
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
    }
}
