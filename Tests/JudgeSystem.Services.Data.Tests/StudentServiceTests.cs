
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Dtos.SchoolClass;
using JudgeSystem.Web.Dtos.Student;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.ViewModels.Student;

using Moq;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class StudentServiceTests : TransientDbContextProvider
    {
        private readonly IPasswordHashService hashService = new PasswordHashService();

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            StudentService service = await CreateStudentService(new List<Student>());
            string activationKey = Guid.NewGuid().ToString();
            var student = new StudentCreateInputModel
            {
                FullName = "Test Testov",
                Email = "test@mail.bg",
                NumberInCalss = 12
            };

            StudentDto actualStudent = await service.Create(student, activationKey);

            Assert.NotNull(actualStudent.Id);
            Assert.False(actualStudent.IsActivated);
            Assert.Equal(hashService.HashPassword(activationKey), actualStudent.ActivationKeyHash);
            Assert.Contains(context.Students, x => x.FullName == student.FullName &&
            x.Email == student.Email && x.NumberInCalss == student.NumberInCalss);
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);
            string email = "student2@mail.bg";
            string studentId = context.Students.First(x => x.Email == email).Id;

            await service.Delete(studentId);

            Assert.False(context.Students.Any(x => x.Email == "student2@mail.bg"));
        }

        [Fact]
        public async Task Delete_WithNonExistingStudent_ShouldThrowError()
        {
            StudentService service = await CreateStudentService(new List<Student>());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Delete(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);
            var student = new Student
            {
                FullName = "mapping test",
                Email = "mapppin@mail.bg",
                NumberInCalss = 10,
                SchoolClass = new SchoolClass
                {
                    ClassNumber = 12,
                    ClassType = SchoolClassType.A
                }
            };
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            StudentProfileViewModel actualData = await service.GetById<StudentProfileViewModel>(student.Id);

            Assert.Equal(student.Email, actualData.Email);
            Assert.Equal(student.FullName, actualData.FullName);
            Assert.Equal(student.NumberInCalss, actualData.NumberInCalss);
            Assert.Equal(student.Id, actualData.Id);
            Assert.Equal("12 A", actualData.SchoolClassName);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetById<StudentDto>(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetStudentClass_WithValidId_ShouldReturnCorrectData()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);
            var student = new Student
            {
                SchoolClass = new SchoolClass
                {
                    ClassNumber = 12,
                    ClassType = SchoolClassType.A
                }
            };
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            SchoolClassDto actualData = await service.GetStudentClass(student.Id);

            Assert.NotNull(actualData);
            Assert.Equal(12, actualData.ClassNumber);
            Assert.Equal(SchoolClassType.A, actualData.ClassType);
        }

        [Fact]
        public async Task GetStudentClass_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetStudentClass(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetStudentProfileByActivationKey_WithValidActivationKey_ShouldReturnCorrectData()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);
            string activationKey = Guid.NewGuid().ToString();
            var student = new Student
            {
                FullName = "mapping test",
                Email = "mapppin@mail.bg",
                NumberInCalss = 10,
                ActivationKeyHash = hashService.HashPassword(activationKey)
            };
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            StudentDto actualData = await service.GetStudentProfileByActivationKey(activationKey);

            Assert.NotNull(actualData);
            Assert.Equal(student.Email, actualData.Email);
            Assert.Equal(student.FullName, actualData.FullName);
            Assert.Equal(student.NumberInCalss, actualData.NumberInCalss);
        }

        [Fact]
        public async Task GetStudentProfileByActivationKey_WithInvalidActivationKey_ShouldReturnNull()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);
            string activationKey = Guid.NewGuid().ToString();

            StudentDto actualData = await service.GetStudentProfileByActivationKey(activationKey);

            Assert.Null(actualData);
        }

        [Fact]
        public async Task SetStudentProfileAsActivated_WithValidUser_ShouldWorkCorrect()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);
            Student student = context.Students.FirstOrDefault(x => x.FullName == "Student 4");

            await service.SetStudentProfileAsActivated(student.Id);

            Assert.True(student.IsActivated);
        }

        [Fact]
        public async Task SetStudentProfileAsActivated_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.SetStudentProfileAsActivated(Guid.NewGuid().ToString() ));
        }

        [Theory]
        [InlineData(10, SchoolClassType.A, "id_1, id_3, id_2")]
        [InlineData(10, SchoolClassType.G, "")]
        [InlineData(8, SchoolClassType.A, "")]
        [InlineData(10, null, "id_1, id_3, id_2, id_7, id_6")]
        [InlineData(11, null, "id_5, id_4")]
        [InlineData(null, SchoolClassType.A, "id_1, id_3, id_2")]
        [InlineData(null, SchoolClassType.B, "id_7, id_6, id_5, id_4, id_8, id_9")]
        [InlineData(null, null, "")]
        public async Task SearchStudentsByClass_WithDifferentInputs_ShouldReturnDifferentOutputs(int? classNumber, 
            SchoolClassType? classType, string expectedIds)
        {
            List<Student> testData = GetTestDataWithSchoolClasses();
            StudentService service = await CreateStudentService(testData);

            IEnumerable<StudentProfileViewModel> actualResult = service.SearchStudentsByClass(classNumber, classType, 1, 20);
            IEnumerable<string> actualIds = actualResult.Select(x => x.Id);

            Assert.Equal(expectedIds, string.Join(", ", actualIds));
        }

        [Theory]
        [InlineData(10, SchoolClassType.A, 3)]
        [InlineData(10, SchoolClassType.G, 0)]
        [InlineData(8, SchoolClassType.A, 0)]
        [InlineData(10, null, 5)]
        [InlineData(11, null, 2)]
        [InlineData(null, SchoolClassType.A, 3)]
        [InlineData(null, SchoolClassType.B, 6)]
        [InlineData(null, null, 0)]
        public async Task StudentsByClassCount_WithDifferentInputs_ShouldReturnDifferentOutputs(int? classNumber,
           SchoolClassType? classType, int expectedCount)
        {
            List<Student> testData = GetTestDataWithSchoolClasses();
            StudentService service = await CreateStudentService(testData);

            int actualCount = service.StudentsByClassCount(classNumber, classType);

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(25, false)]
        public async Task ExistsByClassAndNumber_WithDifferentData_ShouldReturnCorrectResults(int numberInClass, bool expectedResult)
        {
            List<Student> testData = GetTestDataWithSchoolClasses();
            StudentService service = await CreateStudentService(testData);
            int schoolClassId = context.SchoolClasses.First(x => x.ClassNumber == 10 && x.ClassType == SchoolClassType.A).Id;

            bool actualResult = service.ExistsByClassAndNumber(schoolClassId, numberInClass);

            Assert.True(actualResult == expectedResult);
        }

        [Fact]
        public async Task ExistsByEmail_WithStudentWithTheSameEmail_ShouldReturnTrue()
        {
            List<Student> testData = GetTestDataWithSchoolClasses();
            StudentService service = await CreateStudentService(testData);

            bool result = service.ExistsByEmail("student1@mail.bg");

            Assert.True(result);
        }

        [Fact]
        public async Task ExistsByEmail_WithStudentWithEmailCaseInsensitiveEqualToProvidedEmail_ShouldReturnTrue()
        {
            List<Student> testData = GetTestDataWithSchoolClasses();
            StudentService service = await CreateStudentService(testData);

            bool result = service.ExistsByEmail("STUDENT1@mail.bg");

            Assert.True(result);
        }

        [Fact]
        public async Task ExistsByEmail_WithoutStudentWithProvidedEmail_ShouldReturnFalse()
        {
            List<Student> testData = GetTestDataWithSchoolClasses();
            StudentService service = await CreateStudentService(testData);

            bool result = service.ExistsByEmail("Pesho@mail.com");

            Assert.False(result);
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect()
        {
            List<Student> testData = GetTestData();
            StudentService service = await CreateStudentService(testData);
            string id = context.Students.First().Id;
            var inputModel = new StudentEditInputModel
            {
               Id = id,
               Email = "edited@mail.bg",
               FullName = "Edited Edit",
               NumberInCalss = 10,
               SchoolClassId = 12
            };

            await service.Update(inputModel);
            Student actualStudent = context.Students.First(x => x.Id == inputModel.Id);

            Assert.Equal(inputModel.Email, actualStudent.Email);
            Assert.Equal(inputModel.FullName, actualStudent.FullName);
            Assert.Equal(inputModel.NumberInCalss, actualStudent.NumberInCalss);
            Assert.Equal(inputModel.SchoolClassId, actualStudent.SchoolClassId);
        }

        [Fact]
        public async Task Update_WithNonExistingLesson_ShouldThrowArgumentEException()
        {
            var inputModel = new StudentEditInputModel
            {
                Id = "invalid_id",
                Email = "edited@mail.bg",
                FullName = "Edited Edit",
                NumberInCalss = 10,
                SchoolClassId = 12
            };
            StudentService service = await CreateStudentService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Update(inputModel));
        }

        private async Task<StudentService> CreateStudentService(List<Student> testData)
        {
            await context.Students.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IRepository<Student> repository = new EfRepository<Student>(context);
            var service = new StudentService(hashService, repository);
            return service;
        }

        private StudentService CreateStudentServiceWithMockedRepository(IQueryable<Student> testData)
        {
            var reposotiryMock = new Mock<IRepository<Student>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new StudentService(hashService, reposotiryMock.Object);
        }

        private List<Student> GetTestData()
        {
            var students = new List<Student>
            {
                new Student { Email = "student1@mail.bg", FullName="Student 1" },
                new Student { Email = "student2@mail.bg", FullName="Student 2" },
                new Student { Email = "student3@mail.bg", FullName="Student 3" },
                new Student { Email = "student4@mail.bg", FullName="Student 4" },
                new Student { Email = "student5@mail.bg", FullName="Student 5" },
            };
            return students;
        }

        private List<Student> GetTestDataWithSchoolClasses()
        {
            var classes = new List<SchoolClass>()
            {
                new SchoolClass { ClassNumber = 10, ClassType = SchoolClassType.A },
                new SchoolClass { ClassNumber = 11, ClassType = SchoolClassType.B },
                new SchoolClass { ClassNumber = 10, ClassType = SchoolClassType.B },
                new SchoolClass { ClassNumber = 12, ClassType = SchoolClassType.B },
            };

            var students = new List<Student>
            {
                new Student { Id = "id_1", Email = "student1@mail.bg", NumberInCalss= 1, SchoolClass = classes[0] },
                new Student { Id = "id_6", Email = "student6@mail.bg", NumberInCalss= 14, SchoolClass = classes[2] },
                new Student { Id = "id_3", Email = "student3@mail.bg", NumberInCalss= 12, SchoolClass = classes[0] },
                new Student { Id = "id_4", Email = "student4@mail.bg", NumberInCalss= 19, SchoolClass = classes[1] },
                new Student { Id = "id_8", Email = "student8@mail.bg", NumberInCalss= 3, SchoolClass = classes[3] },
                new Student { Id = "id_5", Email = "student5@mail.bg", NumberInCalss= 18, SchoolClass = classes[1] },
                new Student { Id = "id_7", Email = "student7@mail.bg", NumberInCalss= 10, SchoolClass = classes[2] },
                new Student { Id = "id_9", Email = "student9@mail.bg", NumberInCalss= 11, SchoolClass = classes[3] },
                new Student { Id = "id_2", Email = "student2@mail.bg", NumberInCalss= 15, SchoolClass = classes[0] },
            };

            foreach (Student student in students)
            {
                student.User = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = student.Email };
                student.IsActivated = true;
            }

            return students;
        }
    }
}
