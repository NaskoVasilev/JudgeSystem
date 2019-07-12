using JudgeSystem.Data;
using JudgeSystem.Services.Data.Tests.ClassFixtures;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class SingletonDbContextProvider : IClassFixture<MappingsProvider>, IClassFixture<InMemoryDatabaseFactory>
    {
        protected ApplicationDbContext Context { get; set; }

        public SingletonDbContextProvider(InMemoryDatabaseFactory factory)
        {
            this.Context = factory.Context;
        }
    }
}
