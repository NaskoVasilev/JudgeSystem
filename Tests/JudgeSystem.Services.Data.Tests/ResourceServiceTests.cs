using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Infrastructure.Exceptions;
using JudgeSystem.Web.InputModels.Resource;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class ResourceServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            var service = await CreateResourceService(new List<Resource>());
            var resource = new ResourceInputModel
            {
                LessonId = 15,
                Name = "Test resource",
                Id = 1
            };
            string filePath = "mdsaldk654K_judgeSystemDocumentation";

            await service.CreateResource(resource, filePath);

            Assert.Contains(this.context.Resources, x => x.Name == resource.Name &&
            x.LessonId == resource.LessonId && x.Id == resource.Id && x.FilePath == filePath);
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            var testData = GetTestData();
            var service = await CreateResourceService(testData);

            var actualData = await service.GetById(2);
            var expectedResut = testData.First(x => x.Id == 2);

            Assert.Equal(expectedResut.Name, actualData.Name);
            Assert.Equal(expectedResut.LessonId, actualData.LessonId);
            Assert.Equal(expectedResut.FilePath, actualData.FilePath);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldReturnNull()
        {
            var testData = GetTestData();
            var service = await CreateResourceService(testData);

            var actualResult = await service.GetById(132);

            Assert.Null(actualResult);
        }

        [Fact]
        public void LessonResources_WithDataInDbContext_ShouldReturnValidResults()
        {
            var service = CreateResourceServiceWithMockedRepository(GetTestData().AsQueryable());

            var actualResults = service.LessonResources(10);

            Assert.Equal("1, 3", string.Join(", ", actualResults.Select(x => x.Id)));
        }


        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetTestData();
            var service = await CreateResourceService(testData);

            var resource = testData.First(x => x.Id == 1);
            await service.Delete(resource);

            Assert.False(this.context.Resources.Any(x => x.Id == 1));
        }

        [Fact]
        public async Task Delete_WithNonExistingStudent_ShouldThrowError()
        {
            var service = await CreateResourceService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Delete(new Resource { Id = 45 }));
        }

        [Fact]
        public async Task Update_WithValidDataAndFilePath_ShouldWorkCorrectAndSetNewFilePath()
        {
            var testData = GetTestData();
            var service = await CreateResourceService(testData);
            string filePath = "test_file_path";
            var inputModel = new ResourceEditInputModel
            {
                Id = 1,
                Name = "Edited"
            };

            await service.Update(inputModel, filePath);
            var actualResource = context.Resources.First(x => x.Id == inputModel.Id);

            Assert.Equal(inputModel.Name, actualResource.Name);
            Assert.Equal(filePath, actualResource.FilePath);
            Assert.Equal(inputModel.Name, actualResource.Name);
        }

        [Fact]
        public async Task Update_WithValidDataAndNotSepcifiedFilePath_ShouldWorkCorrectAndNotSetNewFilePath()
        {
            var testData = GetTestData();
            var service = await CreateResourceService(testData);
            var inputModel = new ResourceEditInputModel
            {
                Id = 1,
                Name = "Edited"
            };

            string filePath = context.Resources.First(x => x.Id == 1).FilePath;
            await service.Update(inputModel);
            var actualResource = context.Resources.First(x => x.Id == inputModel.Id);

            Assert.Equal(inputModel.Name, actualResource.Name);
            Assert.Equal(filePath, actualResource.FilePath);
            Assert.Equal(inputModel.Name, actualResource.Name);
        }

        [Fact]
        public async Task Update_WithNonExistingLesson_ShouldThrowArgumentEException()
        {
            var inputModel = new ResourceEditInputModel { Id = 854 };
            var service = await CreateResourceService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Update(inputModel));
        }

        private async Task<ResourceService> CreateResourceService(List<Resource> testData)
        {
            await this.context.Resources.AddRangeAsync(testData);
            await this.context.SaveChangesAsync();
            IRepository<Resource> repository = new EfRepository<Resource>(this.context);
            var service = new ResourceService(repository);
            return service;
        }

        private ResourceService CreateResourceServiceWithMockedRepository(IQueryable<Resource> testData)
        {
            var reposotiryMock = new Mock<IRepository<Resource>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new ResourceService(reposotiryMock.Object);
        }

        private List<Resource> GetTestData()
        {
            return new List<Resource>
            {
                new Resource { Id = 1, Name = "Test1", FilePath = "filePath1", LessonId = 10 },
                new Resource { Id = 2, Name = "Test2", FilePath = "filePath2", LessonId = 11 },
                new Resource { Id = 3, Name = "Test3", FilePath = "filePath3", LessonId = 10 },
            };
        }
    }
}
