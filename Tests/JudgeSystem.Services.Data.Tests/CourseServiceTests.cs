using JudgeSystem.Data;
using JudgeSystem.Services.Data.Tests.ClassFixtures;
using JudgeSystem.Services.Data.Tests.Factories;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class CourseServiceTests : IClassFixture<MappingsProvider>
    {
        private readonly ApplicationDbContext context;

        public CourseServiceTests()
        {
            this.context = ApplicationDbContextFactory.CreateInMemoryDatabase();
        }
    }
}
