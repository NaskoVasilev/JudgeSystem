using System.IO;
using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.InputModels.Resource;
using JudgeSystem.Web.Tests.Mocks;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Resource;

using Microsoft.AspNetCore.Http;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class ResourceControllerTests
    {
        [Fact]
        public void Create_ShouldReturnView() =>
          MyController<ResourceController>
          .Instance()
          .Calling(c => c.Create())
          .ShouldReturn()
          .View();

        [Fact]
        public void Create_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<ResourceController>
            .Instance()
            .Calling(c => c.Create(With.Default<ResourceInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(ModelConstants.ResourceNameMaxLength + 1)]
        [InlineData(ModelConstants.ResourceNameMinLength - 1)]
        public void Create_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(int nameLength) =>
            MyController<ResourceController>
            .Instance()
            .Calling(c => c.Create(new ResourceInputModel { Name = new string('a', nameLength), File = null, LessonId = 1 }))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<ResourceInputModel>()
                .ContainingErrorFor(m => m.File)
                .ContainingErrorFor(m => m.Name))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ResourceInputModel>()
                .Passing(model => model.LessonId == 1 && model.Name == new string('a', nameLength)));

        [Theory]
        [InlineData("test.exe")]
        [InlineData("test.bat")]
        [InlineData("test.dll")]
        [InlineData("test.jar")]
        public void Create_WithInvalidFileExtension_ShouldAddErrorInTheModelStateAndReturnTheSameView(string fileName)
        {
            var inputModel = new ResourceInputModel
            {
                File = new FormFile(null, 0, 0, fileName, fileName),
                LessonId = 1,
                PracticeId = 1,
                Name = "valid name"
            };

            MyController<ResourceController>
            .Instance()
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<ResourceInputModel>()
                .ContainingErrorFor(m => m.File)
                .Equals(string.Format(ErrorMessages.UnsupportedFileFormat, Path.GetExtension(fileName))))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ResourceInputModel>()
                .Passing(model => model.LessonId == inputModel.LessonId &&
                model.Name == inputModel.Name &&
                model.PracticeId == inputModel.PracticeId));
        }

        [Fact]
        public void Create_WithValidInputData_ShouldReturnRedirectResultUploadFileAndAddTheResourceInTheDb()
        {
            Lesson lesson = LessonTestData.GetEntity();

            using (var memoryStream = new MemoryStream(new byte[0]))
            {
                var inputModel = new ResourceInputModel
                {
                    File = new FormFile(memoryStream, 0, 0, "test", "test.docx"),
                    LessonId = lesson.Id,
                    PracticeId = lesson.Practice.Id,
                    Name = "Loops - Homework",
                };

                MyController<ResourceController>
                .Instance()
                .WithData(lesson)
                .Calling(c => c.Create(inputModel))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldHave()
                .Data(data => data
                    .WithSet<Resource>(set =>
                    {
                        string filePath = new AzureStorageServiceMock().ConstructFilePath(inputModel.File.FileName, inputModel.Name);
                        bool resourceExists = set.Any(r => r.Name == inputModel.Name &&
                        r.FilePath == filePath &&
                        r.LessonId == inputModel.LessonId);
                        Assert.True(resourceExists);
                    }))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Details", "Lesson", new { id = inputModel.LessonId, inputModel.PracticeId });
            }
        }

        [Fact]
        public static void LessonResources_WithValidArguments_ShouldReturnResourceForPassedLesson()
        {
            int lessonId = 1;
            int practiceId = 10;
            IEnumerable<Resource> allResources = ResourceTestData.GenerateResources();
            var expectedResources = allResources.Where(x => x.LessonId == lessonId).ToList();

            MyController<ResourceController>
           .Instance()
           .WithData(allResources)
           .Calling(c => c.LessonResources(lessonId, practiceId))
           .ShouldReturn()
           .View(result => result
               .WithModelOfType<AllResourcesViewModel>()
               .Passing(model =>
               {
                   var actualResources = model.Resources.ToList();
                   Assert.Equal(lessonId, model.LessonId);
                   Assert.Equal(practiceId, model.PracticeId);
                   Assert.Equal(expectedResources.Count, actualResources.Count());

                   for (int i = 0; i < expectedResources.Count; i++)
                   {
                       Assert.Equal(expectedResources[i].Id, actualResources[i].Id);
                       Assert.Equal(expectedResources[i].Name, actualResources[i].Name);
                   }
               }));
        }

        [Fact]
        public void Edit_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Resource resource = ResourceTestData.GetEntity();

            MyController<ResourceController>
            .Instance()
            .WithData(resource)
            .Calling(c => c.Edit(resource.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ResourceEditInputModel>()
                .Passing(model => model.Id == resource.Id && model.Name == resource.Name));
        }

        [Fact]
        public void Edit_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<ResourceController>
            .Instance()
            .Calling(c => c.Edit(With.Default<ResourceEditInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(ModelConstants.ResourceNameMaxLength + 1)]
        [InlineData(ModelConstants.ResourceNameMinLength - 1)]
        public void Edit_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(int nameLength) =>
             MyController<ResourceController>
             .Instance()
             .Calling(c => c.Edit(new ResourceEditInputModel { Name = new string('a', nameLength), File = null }))
             .ShouldHave()
             .ModelState(modelState => modelState
                 .For<ResourceEditInputModel>()
                 .ContainingErrorFor(m => m.Name))
             .AndAlso()
             .ShouldReturn()
             .View(result => result
                 .WithModelOfType<ResourceEditInputModel>()
                 .Passing(model => model.Name == new string('a', nameLength)));

        [Theory]
        [InlineData("test.exe")]
        [InlineData("test.bat")]
        [InlineData("test.dll")]
        [InlineData("test.jar")]
        public void Edit_WithInvalidFileExtension_ShouldAddErrorInTheModelStateAndReturnTheSameView(string fileName)
        {
            var inputModel = new ResourceEditInputModel
            {
                File = new FormFile(null, 0, 0, fileName, fileName),
                Name = "valid name"
            };

            MyController<ResourceController>
            .Instance()
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<ResourceInputModel>()
                .ContainingErrorFor(m => m.File)
                .Equals(string.Format(ErrorMessages.UnsupportedFileFormat, Path.GetExtension(fileName))))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ResourceEditInputModel>()
                .Passing(model => model.Name == inputModel.Name));
        }

        [Fact]
        public void Edit_WithValidInputDataAndNotPassedFile_ShouldReturnRedirectResultAndUpdateOnlyResourceName()
        {
            Resource resource = ResourceTestData.GetEntity();
            int practiceId = resource.Lesson.Practice.Id;
            string filePath = resource.FilePath;
            var inputModel = new ResourceEditInputModel
            {
                Id = resource.Id,
                Name = "C# OOP resources - edited"
            };

            MyController<ResourceController>
            .Instance()
            .WithData(resource)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .Data(data => data
                .WithSet<Resource>(set =>
                {
                    Resource editedResource = set.FirstOrDefault(c => c.Id == inputModel.Id);
                    Assert.NotNull(editedResource);
                    Assert.Equal(inputModel.Name, editedResource.Name);
                    Assert.Equal(filePath, resource.FilePath);
                }))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(nameof(ResourceController.LessonResources), "Resource", new { lessonId = resource.Lesson.Id, practiceId });
        }

        [Theory]
        [InlineData("")]
        [InlineData("edited")]
        public void Edit_WithValidInputDataAndPassedFile_ShouldReturnRedirectResultUploadNewFileDeleteOldFileAndEditResource(string name)
        {
            Resource resource = ResourceTestData.GetEntity();
            string oldName = resource.Name;
            int practiceId = resource.Lesson.Practice.Id;

            using (var memoryStream = new MemoryStream(new byte[0]))
            {
                var inputModel = new ResourceEditInputModel
                {
                    Id = resource.Id,
                    Name = resource.Name + name,
                    File = new FormFile(memoryStream, 0, 0, "edit", "edit.cs")
                };

                string oldFilePath = resource.FilePath;

                MyController<ResourceController>
                .Instance()
                .WithData(resource)
                .Calling(c => c.Edit(inputModel))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Resource>(set =>
                    {
                        //Assert that old file was deleted
                        Assert.Equal(AzureStorageServiceMock.FilePath, oldFilePath);

                        Resource editedResource = set.FirstOrDefault(c => c.Id == inputModel.Id);
                        Assert.NotNull(editedResource);
                        Assert.Equal(inputModel.Name, editedResource.Name);
                        string filePath = new AzureStorageServiceMock().ConstructFilePath(inputModel.File.FileName, oldName);
                        Assert.Equal(filePath, editedResource.FilePath);
                    }))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(ResourceController.LessonResources), "Resource", new { lessonId = resource.Lesson.Id, practiceId });
            }
        }

        [Fact]
        public void Delete_ShouldHaveAttribtesForPostRequestAndEndpointExceptionFilter() =>
            MyController<ResourceController>
            .Instance()
            .WithData(ResourceTestData.GetEntity())
            .Calling(c => c.Delete(1))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ContainingAttributeOfType<EndpointExceptionFilter>());

        [Fact]
        public void Delete_WithValidArguments_ShouldDeleteTheResourceAndFileAndReturnRedirectResult()
        {
            Resource resource = ResourceTestData.GetEntity();

            MyController<ResourceController>
           .Instance()
           .WithData(resource)
           .Calling(c => c.Delete(resource.Id))
           .ShouldHave()
           .Data(data => data
                .WithSet<Resource>(set =>
                {
                    //Assert that delete method in AzureStorageServiceMock was invoked with resource filePath as argument
                    Assert.Equal(AzureStorageServiceMock.FilePath, resource.FilePath);

                    bool resourceExists = set.Any(c => c.Id == resource.Id);
                    Assert.False(resourceExists);
                }))
            .AndAlso()
            .ShouldReturn()
            .Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, resource.Name));
        }
    }
}
