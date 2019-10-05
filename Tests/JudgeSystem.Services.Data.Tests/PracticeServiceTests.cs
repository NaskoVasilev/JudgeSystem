using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Dtos.Lesson;

using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class PracticeServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task AddUserToPracticeIfNotAdded_WithNonExistingUser_ShouldWorkCorrect()
        {
            IRepository<UserPractice> userPracticeRepository = new EfRepository<UserPractice>(context);
            PracticeService service = await CreatePracticeService(GetTestData(), userPracticeRepository);

            await service.AddUserToPracticeIfNotAdded("test123", 2);

            Assert.Contains(context.UserPractices, x => x.PracticeId == 2 && x.UserId == "test123");
        }

        [Fact]
        public async Task AddUserToPracticeIfNotAdded_WithExistingUserPractice_ShouldDoNothing()
        {
            await context.UserPractices.AddAsync(new UserPractice { UserId = "test", PracticeId = 2 });
            IRepository<UserPractice> userPracticeRepository = new EfRepository<UserPractice>(context);
            PracticeService service = await CreatePracticeService(GetTestData(), userPracticeRepository);

            await service.AddUserToPracticeIfNotAdded("test", 2);

            Assert.True(context.UserPractices.Count() == 1);
        }

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            PracticeService service = await CreatePracticeService(new List<Practice>(), null);

            await service.Create(5);

            Assert.Contains(context.Practices, x => x.LessonId == 5);
        }

        [Fact]
        public async Task GetLesson_WithValidData_ShouldWorkCorrect()
        {
            PracticeService service = await CreatePracticeService(GetTestData(), null);

            LessonDto lesson =  await service.GetLesson(2);

            Assert.Equal(45, lesson.Id);
            Assert.Equal("Test", lesson.Name);
        }

        [Fact]
        public async Task GetLesson_WithInValidData_ShouldThrowEntityNotFoundException()
        {
            PracticeService service = await CreatePracticeService(GetTestData(), null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetLesson(22));
        }

        private async Task<PracticeService> CreatePracticeService(List<Practice> testData, IRepository<UserPractice> userPracticeRepository)
        {
            await context.Practices.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IDeletableEntityRepository<Practice> repository = new EfDeletableEntityRepository<Practice>(context);
            var service = new PracticeService(repository, userPracticeRepository);
            return service;
        }

        private List<Practice> GetTestData()
        {
            var practices = new List<Practice>
            {
                new Practice{ Id = 1, Lesson = new Lesson{ Name = "Test123", Id = 1 } },
                new Practice{ Id = 2, Lesson = new Lesson{ Name = "Test", Id = 45 } },
                new Practice{ Id = 3, Lesson = new Lesson{ Name = "Test321", Id = 3 } },
            };
            return practices;
        }
    }
}
