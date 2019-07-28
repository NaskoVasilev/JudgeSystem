using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class PracticeServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task AddUserToPracticeIfNotAdded_WithNonExistingUser_ShouldWorkCorrect()
        {
            IRepository<UserPractice> userPracticeRepository = new EfRepository<UserPractice>(this.context);
            var service = await CreatePracticeService(GetTestData(), userPracticeRepository);

            await service.AddUserToPracticeIfNotAdded("test123", 2);

            Assert.Contains(this.context.UserPractices, x => x.PracticeId == 2 && x.UserId == "test123");
        }

        [Fact]
        public async Task AddUserToPracticeIfNotAdded_WithExistingUserPractice_ShouldDoNothing()
        {
            await this.context.UserPractices.AddAsync(new UserPractice { UserId = "test", PracticeId = 2 });
            IRepository<UserPractice> userPracticeRepository = new EfRepository<UserPractice>(this.context);
            var service = await CreatePracticeService(GetTestData(), userPracticeRepository);

            await service.AddUserToPracticeIfNotAdded("test", 2);

            Assert.True(this.context.UserPractices.Count() == 1);
        }

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            var service = await CreatePracticeService(new List<Practice>(), null);

            await service.Create(5);

            Assert.Contains(this.context.Practices, x => x.LessonId == 5);
        }

        [Fact]
        public async Task GetLessonId_WithValidData_ShouldWorkCorrect()
        {
            var service = await CreatePracticeService(GetTestData(), null);

            int actualId =  service.GetLessonId(2);

            Assert.Equal(45, actualId);
        }

        [Fact]
        public async Task GetLessonId_WithInValidData_ShouldThrowEntityNotFoundException()
        {
            var service = await CreatePracticeService(GetTestData(), null);

            Assert.Throws<EntityNotFoundException>(() => service.GetLessonId(22));
        }


        private async Task<PracticeService> CreatePracticeService(List<Practice> testData, IRepository<UserPractice> userPracticeRepository)
        {
            await this.context.Practices.AddRangeAsync(testData);
            await this.context.SaveChangesAsync();
            IDeletableEntityRepository<Practice> repository = new EfDeletableEntityRepository<Practice>(this.context);
            var service = new PracticeService(repository, userPracticeRepository);
            return service;
        }

        private List<Practice> GetTestData()
        {
            return new List<Practice>
            {
                new Practice{ Id = 1, LessonId = 1 },
                new Practice{ Id = 2, LessonId = 45 },
                new Practice{ Id = 3, LessonId = 3 },
            };
        }
    }
}
