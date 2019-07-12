using JudgeSystem.Data;
using JudgeSystem.Services.Data.Tests.ClassFixtures;
using JudgeSystem.Services.Data.Tests.Factories;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class BaseServiceTests : IClassFixture<MappingsProvider>
    {
        protected readonly ApplicationDbContext context;

        public BaseServiceTests()
        {
            this.context = ApplicationDbContextFactory.CreateInMemoryDatabase();
        }
    }
}
