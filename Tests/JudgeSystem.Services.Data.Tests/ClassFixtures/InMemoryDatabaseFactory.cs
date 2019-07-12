using System;
using JudgeSystem.Data;
using JudgeSystem.Services.Data.Tests.Factories;

namespace JudgeSystem.Services.Data.Tests.ClassFixtures
{
    public class InMemoryDatabaseFactory : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }

        public InMemoryDatabaseFactory()
        {
            this.Context = ApplicationDbContextFactory.CreateInMemoryDatabase();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
