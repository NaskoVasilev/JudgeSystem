using JudgeSystem.Data;
using JudgeSystem.Services.Data.Tests.ClassFixtures;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class BaseDatabaseFixtureTestClass : IClassFixture<MappingsProvider>, IClassFixture<InMemoryDatabaseFactory>
    {
        protected ApplicationDbContext Context { get; set; }

        public BaseDatabaseFixtureTestClass(InMemoryDatabaseFactory factory)
        {
            this.Context = factory.Context;
        }
    }
}
