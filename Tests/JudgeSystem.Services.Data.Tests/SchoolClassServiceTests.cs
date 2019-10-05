using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Dtos.SchoolClass;

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
            SchoolClassService service = await CreateSchoolClassService(GetTestData());

            bool actualResult = service.ClassExists(classNumber, schoolClassType);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            int classNumber = 12;
            SchoolClassType classType = SchoolClassType.G;
            SchoolClassService service = await CreateSchoolClassService(new List<SchoolClass>());

            SchoolClassDto schoolClass = await service.Create(classNumber, classType);

            Assert.Equal(classNumber, schoolClass.ClassNumber);
            Assert.Equal(classType, schoolClass.ClassType);
            Assert.True(context.SchoolClasses.Any(x => x.ClassNumber == classNumber && x.Id == schoolClass.Id && x.ClassType == classType));
        }

        [Fact]
        public async Task GetAllClasses_WithData_ShouldReturnAllSchoolClasses()
        {
            List<SchoolClass> testData = GetTestData();
            SchoolClassService service = await CreateSchoolClassService(testData);

            IEnumerable<SchoolClassDto> actualResult = service.GetAllClasses();

            foreach (SchoolClassDto schoolClass in actualResult)
            {
                Assert.Contains(testData, x => x.Id == schoolClass.Id && x.ClassNumber == schoolClass.ClassNumber && x.ClassType == schoolClass.ClassType);
            }
        }

        [Fact]
        public async Task GetGrade_WithValidData_ShouldReturnCorrectGrade()
        {
            List<SchoolClass> testData = GetTestData();
            SchoolClassService service = await CreateSchoolClassService(testData);

            string actualResult = await service.GetGrade(2);

            Assert.Equal("10 A", actualResult);
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            List<SchoolClass> testData = GetTestData();
            SchoolClassService service = await CreateSchoolClassService(testData);

            int id = 2;
            SchoolClassDto actualData = await service.GetById<SchoolClassDto>(id);
            SchoolClass expectedData = testData.First(x => x.Id == id);

            Assert.Equal(actualData.Name, expectedData.Name);
            Assert.Equal(actualData.ClassNumber, expectedData.ClassNumber);
            Assert.Equal(actualData.ClassType, expectedData.ClassType);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<SchoolClass> testData = GetTestData();
            SchoolClassService service = await CreateSchoolClassService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetById<SchoolClassDto>(165));
        }

        private async Task<SchoolClassService> CreateSchoolClassService(List<SchoolClass> testData)
        {
            await context.SchoolClasses.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IDeletableEntityRepository<SchoolClass> repository = new EfDeletableEntityRepository<SchoolClass>(context);
            var service = new SchoolClassService(repository);
            return service;
        }

        private List<SchoolClass> GetTestData()
        {
            var classes = new List<SchoolClass>
            {
                new SchoolClass { Id = 2, ClassNumber = 10, ClassType = SchoolClassType.A },
                new SchoolClass { Id = 3, ClassNumber = 10, ClassType = SchoolClassType.B },
                new SchoolClass { Id = 4, ClassNumber = 11, ClassType = SchoolClassType.D },
                new SchoolClass { Id = 5, ClassNumber = 11, ClassType = SchoolClassType.G },
                new SchoolClass { Id = 6, ClassNumber = 12, ClassType = SchoolClassType.A },
            };
            return classes;
        }
    }
}
