using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.ViewModels.Student;
using Microsoft.EntityFrameworkCore;
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
            var student = new Student
            {
                FullName = "Test Testov",
                Email = "test@mail.bg",
                ActivationKeyHash = activationKey,
                NumberInCalss = 12
            };

            var actualStudent = await service.Create(student);

            Assert.NotNull(actualStudent.Id);
            Assert.Equal(hashService.HashPassword(activationKey), actualStudent.ActivationKeyHash);
            Assert.Contains(this.context.Students, x => x.FullName == student.FullName &&
            x.Email == student.Email && x.NumberInCalss == student.NumberInCalss);
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);

            var student = testData.First(x => x.Email == "student2@mail.bg");
            await service.DeleteAsync(student);

            Assert.False(this.context.Students.Any(x => x.Email == "student2@mail.bg"));
        }

        [Fact]
        public async Task Delete_WithNonExistingStudent_ShouldThrowError()
        {
            var service = await CreateStudentService(new List<Student>());

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => service.DeleteAsync(new Student { Id = Guid.NewGuid().ToString() }));
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
        public async Task GetById_WithInValidId_ShouldReturnNull()
        {
            var testData = GetTestData();
            var service = await CreateStudentService(testData);

            var actualData = await service.GetById(Guid.NewGuid().ToString());

            Assert.Null(actualData);
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

            var actualData = await service.GetStudentClassAsync(student.Id);

            Assert.NotNull(actualData);
            Assert.Equal(12, actualData.ClassNumber);
            Assert.Equal(SchoolClassType.A, actualData.ClassType);
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
    }
}
