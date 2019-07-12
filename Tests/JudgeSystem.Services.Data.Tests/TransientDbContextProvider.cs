using JudgeSystem.Data;
using JudgeSystem.Services.Data.Tests.ClassFixtures;
using JudgeSystem.Services.Data.Tests.Factories;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class TransientDbContextProvider : IClassFixture<MappingsProvider>
    {
        protected readonly ApplicationDbContext context;

        public TransientDbContextProvider()
        {
            this.context = ApplicationDbContextFactory.CreateInMemoryDatabase();
        }
    }
}
