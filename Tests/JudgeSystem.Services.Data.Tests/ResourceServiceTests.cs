using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Dtos.Resource;
using JudgeSystem.Web.InputModels.Resource;
using JudgeSystem.Web.ViewModels.Resource;

using Moq;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class ResourceServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            ResourceService service = await CreateResourceService(new List<Resource>());
            var resource = new ResourceInputModel
            {
                LessonId = 15,
                Name = "Test resource",
                OrderBy = 10,
            };
            string filePath = "mdsaldk654K_judgeSystemDocumentation";

            await service.CreateResource(resource, filePath);

            Assert.Contains(context.Resources, x => x.Name == resource.Name &&
            x.LessonId == resource.LessonId && x.Id == 1);
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            List<Resource> testData = GetTestData();
            ResourceService service = await CreateResourceService(testData);

            ResourceDto actualData = await service.GetById<ResourceDto>(2);
            Resource expectedResut = testData.First(x => x.Id == 2);

            Assert.Equal(expectedResut.Name, actualData.Name);
            Assert.Equal(expectedResut.LessonId, actualData.LessonId);
            Assert.Equal(expectedResut.FilePath, actualData.FilePath);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldReturnNull()
        {
            List<Resource> testData = GetTestData();
            ResourceService service = await CreateResourceService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetById<ResourceDto>(345));
        }

        [Fact]
        public void LessonResources_WithDataInDbContext_ShouldReturnValidResults()
        {
            ResourceService service = CreateResourceServiceWithMockedRepository(GetTestData().AsQueryable());

            IEnumerable<ResourceViewModel> actualResults = service.LessonResources(10);

            Assert.Equal("1, 3", string.Join(", ", actualResults.Select(x => x.Id)));
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            List<Resource> testData = GetTestData();
            ResourceService service = await CreateResourceService(testData);

            await service.Delete(1);

            Assert.False(context.Resources.Any(x => x.Id == 1));
        }

        [Fact]
        public async Task Delete_WithNonExistingStudent_ShouldThrowError()
        {
            ResourceService service = await CreateResourceService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Delete(45));
        }

        [Fact]
        public async Task Update_WithValidDataAndFilePath_ShouldWorkCorrectAndSetNewFilePath()
        {
            List<Resource> testData = GetTestData();
            ResourceService service = await CreateResourceService(testData);
            string filePath = "test_file_path";
            var inputModel = new ResourceEditInputModel
            {
                Id = 1,
                Name = "Edited"
            };

            await service.Update(inputModel, filePath);
            Resource actualResource = context.Resources.First(x => x.Id == inputModel.Id);

            Assert.Equal(inputModel.Name, actualResource.Name);
            Assert.Equal(filePath, actualResource.FilePath);
            Assert.Equal(inputModel.Name, actualResource.Name);
        }

        [Fact]
        public async Task Update_WithValidDataAndNotSepcifiedFilePath_ShouldWorkCorrectAndNotSetNewFilePath()
        {
            List<Resource> testData = GetTestData();
            ResourceService service = await CreateResourceService(testData);
            var inputModel = new ResourceEditInputModel
            {
                Id = 1,
                Name = "Edited"
            };

            string filePath = context.Resources.First(x => x.Id == 1).FilePath;
            await service.Update(inputModel);
            Resource actualResource = context.Resources.First(x => x.Id == inputModel.Id);

            Assert.Equal(inputModel.Name, actualResource.Name);
            Assert.Equal(filePath, actualResource.FilePath);
            Assert.Equal(inputModel.Name, actualResource.Name);
        }

        [Fact]
        public async Task Update_WithNonExistingLesson_ShouldThrowArgumentEException()
        {
            var inputModel = new ResourceEditInputModel { Id = 854 };
            ResourceService service = await CreateResourceService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Update(inputModel));
        }

        private async Task<ResourceService> CreateResourceService(List<Resource> testData)
        {
            await context.Resources.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IRepository<Resource> repository = new EfRepository<Resource>(context);
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
            var resources = new List<Resource>
            {
                new Resource { Id = 1, Name = "Test1", FilePath = "filePath1", LessonId = 10 },
                new Resource { Id = 2, Name = "Test2", FilePath = "filePath2", LessonId = 11 },
                new Resource { Id = 3, Name = "Test3", FilePath = "filePath3", LessonId = 10 },
            };
            return resources;
        }
    }
}
