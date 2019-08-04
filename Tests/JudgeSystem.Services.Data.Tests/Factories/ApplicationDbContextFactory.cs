using System;

using JudgeSystem.Data;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data.Tests.Factories
{
    public static class ApplicationDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryDatabase()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
