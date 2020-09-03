using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Student;

using Moq;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class StudentControllerTests
    {
        [Fact]
        public void Create_ShouldReturnView() =>
          MyController<StudentController>
          .Instance()
          .Calling(c => c.Create())
          .ShouldReturn()
          .View();

        [Fact]
        public void Create_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<StudentController>
            .Instance()
            .Calling(c => c.Create(With.Default<StudentCreateInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(ModelConstants.StudentFullNameMinLength - 1, null, GlobalConstants.MaxStudentsInClass + 1)]
        [InlineData(ModelConstants.StudentFullNameMaxLength + 1, "notvalidmail", GlobalConstants.MinStudentsInClass - 1)]
        public void Create_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(
            int nameLength,
            string email,
            int numberInClass)
        {
            var inputModel = new StudentCreateInputModel
            {
                Email = email,
                FullName = new string('a', nameLength),
                NumberInCalss = numberInClass,
                SchoolClassId = 1
            };

            MyController<StudentController>
            .Instance()
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<StudentCreateInputModel>()
                .ContainingErrorFor(m => m.FullName)
                .ContainingErrorFor(m => m.NumberInCalss)
                .ContainingErrorFor(m => m.Email))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<StudentCreateInputModel>()
                .Passing(model => model.FullName == inputModel.FullName &&
                         model.NumberInCalss == inputModel.NumberInCalss &&
                         model.Email == inputModel.Email));
        }

        [Fact]
        public void Create_WithUsedStudentEmail_ShouldAddErrorMessageInTheModelState()
        {
            Student student = StudentTestData.GetEntity();
            var inputModel = new StudentCreateInputModel
            {
                Email = student.Email,
                FullName = "test name",
                NumberInCalss = 12
            };

            MyController<StudentController>
            .Instance()
            .WithData(student)
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<StudentCreateInputModel>()
                .ContainingErrorFor(m => m.Email)
                .Equals(ErrorMessages.StudentWithTheSameEmailAlreadyExists));
        }

        [Fact]
        public void Create_WithUsedGradeAndNumberInClass_ShouldAddErrorMessageInTheModelState()
        {
            Student student = StudentTestData.GetEntity();
            string grade = $"{student.SchoolClass.ClassNumber} {student.SchoolClass.ClassType}";
            var inputModel = new StudentCreateInputModel
            {
                Email = "some@email.com",
                FullName = "test name",
                NumberInCalss = student.NumberInCalss,
                SchoolClassId = student.SchoolClass.Id
            };

            MyController<StudentController>
            .Instance()
            .WithData(student)
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<StudentCreateInputModel>()
                .ContainingErrorFor(m => m.NumberInCalss)
                .Equals(string.Format(ErrorMessages.StudentWithTheSameNumberInClassExists, grade)));
        }

        [Fact]
        public void Create_WithValidInputData_ShouldReturnRedirectResultAndShoudAddTheStudentInTheDb()
        {
            Student student = StudentTestData.GetEntity();
            var inputModel = new StudentCreateInputModel
            {
                Email = "test@mail.com",
                FullName = "Test Student",
                NumberInCalss = 12,
                SchoolClassId = student.SchoolClass.Id
            };

            string activationKey = "test activation key";
            string activationKeyHash = new PasswordHashService().HashPassword(activationKey);

            var studentProfileServiceMock = new Mock<IStudentProfileService>();
            studentProfileServiceMock.Setup(x => x.SendActivationEmail(inputModel.Email, It.IsAny<string>()))
                .Returns(Task.FromResult(activationKey));


            MyController<StudentController>
            .Instance()
            .WithDependencies(
                From.Services<IStudentService>(),
                From.Services<ISchoolClassService>(),
                studentProfileServiceMock.Object,
                From.Services<IPaginationService>(),
                From.Services<IRouteBuilder>())
            .WithData(student.SchoolClass)
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .Data(data => data
                .WithSet<Student>(set =>
                {
                    Student createdStudent = set.FirstOrDefault(s => s.Email == inputModel.Email &&
                    s.FullName == inputModel.FullName &&
                    s.NumberInCalss == inputModel.NumberInCalss &&
                    s.SchoolClassId == inputModel.SchoolClassId &&
                    s.ActivationKeyHash == activationKeyHash);

                    Assert.NotNull(createdStudent);
                    studentProfileServiceMock.Verify(x => x.SendActivationEmail(inputModel.Email, It.IsAny<string>()), Times.Once);
                }))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(nameof(StudentController.StudentsByClass),
                new { classNumber = student.SchoolClass.ClassNumber, classType = student.SchoolClass.ClassType });
        }

        [Fact]
        public void Edit_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Student student = StudentTestData.GetEntity();

            MyController<StudentController>
            .Instance()
            .WithData(student)
            .Calling(c => c.Edit(student.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<StudentEditInputModel>()
                .Passing(model =>
                {
                    Assert.Equal(student.Id, model.Id);
                    Assert.Equal(student.Email, model.Email);
                    Assert.Equal(student.FullName, model.FullName);
                    Assert.Equal(student.NumberInCalss, model.NumberInCalss);
                    Assert.Equal(student.SchoolClassId, model.SchoolClassId);
                }));
        }

        [Fact]
        public void Edit_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken()
        {
            Student student = StudentTestData.GetEntity();
            var inputModel = new StudentEditInputModel
            {
                Id = student.Id
            };

            MyController<StudentController>
            .Instance()
            .WithData(student)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());
        }

        [Theory]
        [InlineData(ModelConstants.StudentFullNameMinLength - 1, null, GlobalConstants.MaxStudentsInClass + 1)]
        [InlineData(ModelConstants.StudentFullNameMaxLength + 1, "notvalidmail", GlobalConstants.MinStudentsInClass - 1)]
        public void Edit_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(
            int nameLength,
            string email,
            int numberInClass)
        {
            Student student = StudentTestData.GetEntity();
            var inputModel = new StudentEditInputModel
            {
                Id = student.Id,
                Email = email,
                FullName = new string('a', nameLength),
                NumberInCalss = numberInClass,
                SchoolClassId = 1
            };

            MyController<StudentController>
            .Instance()
            .WithData(student)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<StudentEditInputModel>()
                .ContainingErrorFor(m => m.FullName)
                .ContainingErrorFor(m => m.NumberInCalss)
                .ContainingErrorFor(m => m.Email))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<StudentEditInputModel>()
                .Passing(model => model.FullName == inputModel.FullName &&
                         model.NumberInCalss == inputModel.NumberInCalss &&
                         model.Email == inputModel.Email &&
                         model.Id == inputModel.Id));
        }

        [Fact]
        public void Edit_WithUsedStudentEmail_ShouldAddErrorMessageInTheModelState()
        {
            Student student = StudentTestData.GetEntity();
            var otherStudent = new Student
            {
                Id = "id_2",
                Email = "duplicate@email.com",
                SchoolClassId = student.SchoolClassId,
                NumberInCalss = student.NumberInCalss
            };

            var inputModel = new StudentEditInputModel
            {
                Id = student.Id,
                Email = otherStudent.Email,
                FullName = "test name",
                NumberInCalss = student.NumberInCalss,
                SchoolClassId = student.SchoolClass.Id
            };

            MyController<StudentController>
            .Instance()
            .WithData(student, otherStudent)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<StudentEditInputModel>()
                .ContainingErrorFor(m => m.Email)
                .Equals(ErrorMessages.StudentWithTheSameEmailAlreadyExists));
        }

        [Theory]
        [InlineData(2, 10)]
        [InlineData(20, 1)]
        [InlineData(22, 7)]
        public void Edit_WithUsedGradeAndNumberInClass_ShouldAddErrorMessageInTheModelState(int numberInClass, int schoolClassId)
        {
            Student student = StudentTestData.GetEntity();
            var otherStudent = new Student
            {
                Id = "id_2",
                NumberInCalss = numberInClass,
                Email = student.Email,
                SchoolClass = new SchoolClass
                {
                    Id = schoolClassId,
                    ClassNumber = 12,
                    ClassType = SchoolClassType.A
                }
            };

            string grade = $"{otherStudent.SchoolClass.ClassNumber} {otherStudent.SchoolClass.ClassType}";
            var inputModel = new StudentEditInputModel
            {
                Id = student.Id,
                Email = "some@email.com",
                FullName = "test name",
                NumberInCalss = otherStudent.NumberInCalss,
                SchoolClassId = otherStudent.SchoolClass.Id
            };

            MyController<StudentController>
            .Instance()
            .WithData(student, otherStudent)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<StudentEditInputModel>()
                .ContainingErrorFor(m => m.NumberInCalss)
                .Equals(string.Format(ErrorMessages.StudentWithTheSameNumberInClassExists, grade)));
        }

        [Theory]
        [InlineData("other@email.com", 2, 1)]
        [InlineData("test@test.me", 20, 1)]
        [InlineData("test@test.me", 2, 10)]
        [InlineData("test@test.me", 22, 10)]
        [InlineData("test@test.edited", 22, 7)]
        public void Edit_WithValidInputData_ShouldReturnRedirectResultAndShoudEditTheStudent(string email, int numberInClass, int schoolClassId)
        {
            Student student = StudentTestData.GetEntity();
            var entities = new List<object> { student };

            if (student.SchoolClass.Id != schoolClassId)
            {
                var schoolClass = new SchoolClass
                {
                    Id = schoolClassId,
                    ClassNumber = 10,
                    ClassType = SchoolClassType.A
                };

                entities.Add(schoolClass);
            }
            // NumberInCalss = 2; SchoolClass.Id = 1; Email = "test@test.me"
            var inputModel = new StudentEditInputModel
            {
                Id = student.Id,
                Email = email,
                FullName = "Test Student",
                NumberInCalss = numberInClass,
                SchoolClassId = schoolClassId
            };

            MyController<StudentController>
            .Instance()
            .WithData(entities)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .Data(data => data
                .WithSet<Student>(set =>
                {
                    Student editedStudent = set.FirstOrDefault(s => s.Email == inputModel.Email &&
                    s.FullName == inputModel.FullName &&
                    s.NumberInCalss == inputModel.NumberInCalss &&
                    s.SchoolClassId == inputModel.SchoolClassId);

                    Assert.NotNull(editedStudent);
                }))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(nameof(StudentController.StudentsByClass),
                new { classNumber = student.SchoolClass.ClassNumber, classType = student.SchoolClass.ClassType });
        }

        [Fact]
        public void Delete_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Student student = StudentTestData.GetEntity();

            MyController<StudentController>
            .Instance()
            .WithData(student)
            .Calling(c => c.Delete(student.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<StudentProfileViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(student.Id, model.Id);
                    Assert.Equal(student.Email, model.Email);
                    Assert.Equal(student.FullName, model.FullName);
                    Assert.Equal(student.NumberInCalss, model.NumberInCalss);
                }));
        }

        [Fact]
        public void DeletePost_ShouldHaveAttribtesForPostRequestActionNameAndAntiForgeryToken()
        {
            Student student = StudentTestData.GetEntity();

            MyController<StudentController>
            .Instance()
            .WithData(student)
            .Calling(c => c.DeletePost(student.Id))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken()
                .SpecifyingActionName(nameof(StudentController.Delete)));
        }

        [Fact]
        public void DeletePost_WithStudentWithTheSameIdInTheDb_ShouldDeleteTheStudentAndReturnRedirectResult()
        {
            Student student = StudentTestData.GetEntity();

            MyController<StudentController>
           .Instance()
           .WithData(student)
           .Calling(c => c.DeletePost(student.Id))
           .ShouldHave()
           .Data(data => data
                .WithSet<Student>(set =>
                {
                    bool studentExists = set.Any(c => c.Id == student.Id);
                    Assert.False(studentExists);
                }))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(nameof(StudentController.StudentsByClass),
                new { classNumber = student.SchoolClass.ClassNumber, classType = student.SchoolClass.ClassType });
        }

        [Theory]
        [InlineData(12, SchoolClassType.B)]
        [InlineData(11, null)]
        [InlineData(null, SchoolClassType.A)]
        public static void StudentsByClass_WithDifferentArgumnets_ShoudReturnFilteredStudents(int? grade, SchoolClassType? schoolClassType)
        {
            IEnumerable<Student> students = StudentTestData.GetEntities();
            var expectedStudents = new List<Student>(students).OrderBy(x => x.Id).ToList();
            if (grade.HasValue)
            {
                expectedStudents = expectedStudents.Where(x => x.SchoolClass.ClassNumber == grade.Value).ToList();
            }

            if (schoolClassType.HasValue)
            {
                expectedStudents = expectedStudents.Where(x => x.SchoolClass.ClassType == schoolClassType.Value).ToList();
            }

            MyController<StudentController>
           .Instance()
           .WithData(students)
           .Calling(c => c.StudentsByClass(grade, schoolClassType, 1))
           .ShouldReturn()
           .View(result => result
               .WithModelOfType<StudentsByClassViewModel>()
               .Passing(model =>
               {
                   var actualStudents = model.Students.OrderBy(x => x.Id).ToList();
                   Assert.Equal(1, model.PaginationData.CurrentPage);
                   Assert.Equal(1, model.PaginationData.NumberOfPages);
                   Assert.Equal(expectedStudents.Count, actualStudents.Count());

                   for (int i = 0; i < expectedStudents.Count; i++)
                   {
                       string schoolClassName = $"{expectedStudents[i].SchoolClass.ClassNumber} {expectedStudents[i].SchoolClass.ClassType}";
                       Assert.Equal(expectedStudents[i].Id, actualStudents[i].Id);
                       Assert.Equal(expectedStudents[i].FullName, actualStudents[i].FullName);
                       Assert.Equal(expectedStudents[i].Email, actualStudents[i].Email);
                       Assert.Equal(schoolClassName, actualStudents[i].SchoolClassName);
                   }
               }));
        }
    }
}
