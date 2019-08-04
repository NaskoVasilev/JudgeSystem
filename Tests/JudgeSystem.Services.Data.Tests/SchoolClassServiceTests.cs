using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;

using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class SchoolClassServiceTests : TransientDbContextProvider
    {
        [Theory]
        [InlineData(10, SchoolClassType.A, true)]
        [InlineData(11, SchoolClassType.A, false)]
        [InlineData(13, SchoolClassType.A, false)]
        public async Task ClassExists_WithDifferentData_ShouldReturnCorrectValues(int classNumber, SchoolClassType schoolClassType, bool expectedResult)
        {
            var service = await CreateSchoolClassService(GetTestData());

            bool actualResult = service.ClassExists(classNumber, schoolClassType);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            int classNumber = 12;
            SchoolClassType classType = SchoolClassType.G;
            var service = await CreateSchoolClassService(GetTestData());

            var schoolClass = await service.Create(classNumber, classType);

            Assert.Equal(classNumber, schoolClass.ClassNumber);
            Assert.Equal(classType, schoolClass.ClassType);
            Assert.True(this.context.SchoolClasses.Any(x => x.ClassNumber == classNumber && x.Id == schoolClass.Id && x.ClassType == classType));
        }

        [Fact]
        public async Task GetAllClasses_WithData_ShouldReturnAllSchoolClasses()
        {
            var testData = GetTestData();
            var service = await CreateSchoolClassService(testData);

            var actualResult = service.GetAllClasses();

            foreach (var schoolClass in actualResult)
            {
                Assert.Contains(testData, x => x.Id == schoolClass.Id && x.ClassNumber == schoolClass.ClassNumber && x.ClassType == schoolClass.ClassType);
            }
        }

        private async Task<SchoolClassService> CreateSchoolClassService(List<SchoolClass> testData)
        {
            await this.context.SchoolClasses.AddRangeAsync(testData);
            await this.context.SaveChangesAsync();
            IDeletableEntityRepository<SchoolClass> repository = new EfDeletableEntityRepository<SchoolClass>(this.context);
            var service = new SchoolClassService(repository);
            return service;
        }

        private List<SchoolClass> GetTestData()
        {
            return new List<SchoolClass>
            {
                new SchoolClass { Id = 2, ClassNumber = 10, ClassType = SchoolClassType.A },
                new SchoolClass { Id = 3, ClassNumber = 10, ClassType = SchoolClassType.B },
                new SchoolClass { Id = 4, ClassNumber = 11, ClassType = SchoolClassType.D },
                new SchoolClass { Id = 5, ClassNumber = 11, ClassType = SchoolClassType.G },
                new SchoolClass { Id = 6, ClassNumber = 12, ClassType = SchoolClassType.A },
            };
        }
    }
}
