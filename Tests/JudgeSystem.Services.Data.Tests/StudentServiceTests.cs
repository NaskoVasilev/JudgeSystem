using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Dtos.Student;
using JudgeSystem.Web.Infrastructure.Exceptions;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.ViewModels.Student;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class StudentServiceTests : TransientDbContextProvider
    {
        private readonly IPasswordHashService hashService = new PasswordHashService();

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            var service = await CreateStudentService(new List<Student>());
            string activationKey = Guid.NewGuid().ToString();
            var student = new StudentCreateInputModel
            {
                FullName = "Test Testov",
                Email = "test@mail.bg",
                NumberInCalss = 12
            };

            var actualStudent = await service.Create(student, activationKey);

            Assert.NotNull(actualStudent.Id);
            Assert.False(actualStudent.IsActivated);
            Assert.Equal(hashService.HashPassword(activationKey), actualStudent.ActivationKeyHash);
            Assert.Contains(this.context.Students, x => x.FullName == student.FullName &&
            x.Email == student.Email && x.NumberInCalss == student.NumberInCalss);
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);
            string email = "student2@mail.bg";
            var studentId = context.Students.First(x => x.Email == email).Id;

            await service.Delete(studentId);

            Assert.False(this.context.Students.Any(x => x.Email == "student2@mail.bg"));
        }

        [Fact]
        public async Task Delete_WithNonExistingStudent_ShouldThrowError()
        {
            var service = await CreateStudentService(new List<Student>());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Delete(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);
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

            var actualData = await service.GetById<StudentProfileViewModel>(student.Id);

            Assert.Equal(student.Email, actualData.Email);
            Assert.Equal(student.FullName, actualData.FullName);
            Assert.Equal(student.NumberInCalss, actualData.NumberInCalss);
            Assert.Equal(student.Id, actualData.Id);
            Assert.Equal("12 A", actualData.SchoolClassName);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetById<StudentDto>(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetStudentClassAsync_WithValidId_ShouldReturnCorrectData()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);
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

            var actualData = await service.GetStudentClass(student.Id);

            Assert.NotNull(actualData);
            Assert.Equal(12, actualData.ClassNumber);
            Assert.Equal(SchoolClassType.A, actualData.ClassType);
        }

        [Fact]
        public async Task GetStudentClassAsync_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetStudentClass(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetStudentProfileByActivationKey_WithValidActivationKey_ShouldReturnCorrectData()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);
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

            var actualData = await service.GetStudentProfileByActivationKey(activationKey);

            Assert.NotNull(actualData);
            Assert.Equal(student.Email, actualData.Email);
            Assert.Equal(student.FullName, actualData.FullName);
            Assert.Equal(student.NumberInCalss, actualData.NumberInCalss);
        }

        [Fact]
        public async Task GetStudentProfileByActivationKey_WithInvalidActivationKey_ShouldReturnNull()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);
            string activationKey = Guid.NewGuid().ToString();

            var actualData = await service.GetStudentProfileByActivationKey(activationKey);

            Assert.Null(actualData);
        }

        [Fact]
        public async Task SetStudentProfileAsActivated_WithValidUser_ShouldWorkCorrect()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);
            var student = this.context.Students.FirstOrDefault(x => x.FullName == "Student 4");

            await service.SetStudentProfileAsActivated(student.Id);

            Assert.True(student.IsActivated);
        }

        [Fact]
        public async Task SetStudentProfileAsActivated_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);

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
            var testData = GetTestDataWithSchoolClasses();
            var service = await CreateStudentService(testData);

            var actualResult = service.SearchStudentsByClass(classNumber, classType);
            var actualIds = actualResult.Select(x => x.Id);

            Assert.Equal(expectedIds, string.Join(", ", actualIds));
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);
            var id = this.context.Students.First().Id;
            var inputModel = new StudentEditInputModel
            {
               Id = id,
               Email = "edited@mail.bg",
               FullName = "Edited Edit",
               NumberInCalss = 10,
               SchoolClassId = 12
            };

            await service.Update(inputModel);
            var actualStudent = context.Students.First(x => x.Id == inputModel.Id);

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
            var service = await CreateStudentService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Update(inputModel));
        }

        private async Task<StudentService> CreateStudentService(List<Student> testData)
        {
            await this.context.Students.AddRangeAsync(testData);
            await this.context.SaveChangesAsync();
            IRepository<Student> repository = new EfRepository<Student>(this.context);
            var service = new StudentService(this.hashService, repository);
            return service;
        }

        private StudentService CreateStudentServiceWithMockedRepository(IQueryable<Student> testData)
        {
            var reposotiryMock = new Mock<IRepository<Student>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new StudentService(this.hashService, reposotiryMock.Object);
        }

        private List<Student> GetTestData()
        {
            return new List<Student>
            {
                new Student { Email = "student1@mail.bg", FullName="Student 1" },
                new Student { Email = "student2@mail.bg", FullName="Student 2" },
                new Student { Email = "student3@mail.bg", FullName="Student 3" },
                new Student { Email = "student4@mail.bg", FullName="Student 4" },
                new Student { Email = "student5@mail.bg", FullName="Student 5" },
            };
        }

        private List<Student> GetTestDataWithSchoolClasses()
        {
            List<SchoolClass> classes = new List<SchoolClass>()
            {
                new SchoolClass { ClassNumber = 10, ClassType = SchoolClassType.A },
                new SchoolClass { ClassNumber = 11, ClassType = SchoolClassType.B },
                new SchoolClass { ClassNumber = 10, ClassType = SchoolClassType.B },
                new SchoolClass { ClassNumber = 12, ClassType = SchoolClassType.B },
            };

            return new List<Student>
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
        }
    }
}
