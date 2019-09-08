﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Web.InputModels.Problem;
using JudgeSystem.Web.ViewModels.Search;
using JudgeSystem.Web.ViewModels.Problem;

using Moq;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class ProblemServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task Create_WithValidData_ShouldAddNewProblemToDatabase()
        {
            ProblemService service = await CreateProblemService(new List<Problem>());
            var model = new ProblemInputModel
            {
                IsExtraTask = false,
                LessonId = 5,
                MaxPoints = 100,
                Name = "test"
            };

            ProblemDto actualProblem = await service.Create(model);

            Assert.True(context.Problems.Any(x => x.Name == "test"));
            Assert.Equal(5, actualProblem.LessonId);
            Assert.Equal(100, actualProblem.MaxPoints);
            Assert.False(actualProblem.IsExtraTask);
            Assert.Equal("test", actualProblem.Name);
            Assert.True(actualProblem.Id > 0);
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            int problemId = 2;
            List<Problem> testData = GetTestData();
            ProblemService problemService = await CreateProblemService(testData);

            await problemService.Delete(problemId);

            Assert.False(context.Problems.Any(x => x.Id == problemId));
        }

        [Fact]
        public async Task Delete_WithNonExistingProblem_ShouldThrowError()
        {
            ProblemService problemService = await CreateProblemService(new List<Problem>()); 

            await Assert.ThrowsAsync<EntityNotFoundException>(() => problemService.Delete(54984));
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            List<Problem> testData = GetTestData();
            ProblemService service = await CreateProblemService(testData);

            int id = 2;
            ProblemDto actualData = await service.GetById<ProblemDto>(id);
            Problem expectedData = testData[id - 1];

            Assert.Equal(actualData.Name, expectedData.Name);
            Assert.Equal(actualData.MaxPoints, expectedData.MaxPoints);
            Assert.Equal(actualData.IsExtraTask, expectedData.IsExtraTask);
            Assert.Equal(actualData.LessonId, expectedData.LessonId);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Problem> testData = GetTestData();
            ProblemService service = await CreateProblemService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetById<ProblemEditInputModel>(165));
        }

        [Theory]
        [InlineData(2, "test2, test4")]
        [InlineData(1, "test1")]
        [InlineData(45, "")]
        public void LessonProblems_WithDifferentData_ShouldWorkCorrect(int lessonId, string expectedNames)
        {
            ProblemService service = CreateProblemServiceWithMockedRepository(GetTestData().AsQueryable());

            IEnumerable<LessonProblemViewModel> actualData = service.LessonProblems(lessonId);

            Assert.Equal(expectedNames, string.Join(", ", actualData.Select(x => x.Name)));
        }

        [Theory]
        [InlineData("C#", "c# WEb API, c# web - asp.NET CoRe")]
        [InlineData("web", "c# WEb API, c# web - asp.NET CoRe")]
        [InlineData("test", "test1, test2, test3, test4")]
        [InlineData("webasp", "")]
        [InlineData("asp", "c# web - asp.NET CoRe")]
        [InlineData("es", "test1, test2, test3, test4")]
        [InlineData("csharp", "")]
        [InlineData("1", "test1")]
        public async Task SearchByName_WithDifferentInputs_ShouldWorkCorrect(string keyword, string expectedResult)
        {
            var lesson = new Lesson() { Id = 1, Name = "lesson1", Practice = new Practice() };
            List<Problem> data = GetTestData();
            foreach (Problem problem in data)
            {
                problem.Lesson = lesson;
            }
            ProblemService service = await CreateProblemService(data);
            await context.Problems.AddRangeAsync(new List<Problem>
            {
                new Problem { Id = 5, Name = "programming basics", Lesson = lesson },
                new Problem { Id = 6, Name = "c# WEb API", Lesson = lesson },
                new Problem { Id = 7, Name = "c# web - asp.NET CoRe", Lesson = lesson },
            });
            await context.SaveChangesAsync();

            IEnumerable<SearchProblemViewModel> actualResult = service.SearchByName(keyword);

            Assert.Equal(expectedResult, string.Join(", ", actualResult.Select(x => x.Name)));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task SearchByName_WithDifferentInvalidInput_ShouldThrowBadRequestException(string keyword)
        {
            ProblemService service = await CreateProblemService(GetTestData());
            await context.SaveChangesAsync();

            BadRequestException exception = Assert.Throws<BadRequestException>(() => service.SearchByName(keyword));
            Assert.Equal(exception.Message, ErrorMessages.InvalidSearchKeyword);
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect()
        {
            List<Problem> testData = GetTestData();
            ProblemService service = await CreateProblemService(testData);
            var inputModel = new ProblemEditInputModel
            {
                Id = 3,
                IsExtraTask = true,
                MaxPoints = 220,
                Name = "edited"
            };

            await service.Update(inputModel);
            Problem actualProblem = context.Problems.First(x => x.Id == inputModel.Id);

            Assert.Equal("edited", actualProblem.Name);
            Assert.True(actualProblem.IsExtraTask);
            Assert.Equal(220, actualProblem.MaxPoints);
            Assert.Equal("edited", actualProblem.Name);
        }

        [Fact]
        public async Task Update_WithNonExistingLesson_ShouldThrowEntityNotFoundException()
        {
            var inputModel = new ProblemEditInputModel
            {
                Id = 45,
                IsExtraTask = true,
                MaxPoints = 220,
                Name = "edited"
            };
            ProblemService service = await CreateProblemService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Update(inputModel));
        }

        [Fact]
        public async Task GetLessonId_WithValidProblemId_ShouldReturnCorrectData()
        {
            ProblemService service = await CreateProblemService(GetTestData());

            int actualResult = await service.GetLessonId(4);

            Assert.Equal(2, actualResult);
        }

        [Fact]
        public async Task GetLessonId_WithInValidId_ShouldReturnThrowEntityNotFoundException()
        {
            List<Problem> testData = GetTestData();
            ProblemService service = await CreateProblemService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetLessonId(161651));
        }


        [Fact]
        public async Task GetProblemName_WithValidId_ShouldReturnCorrectData()
        {
            List<Problem> testData = GetTestData();
            ProblemService service = await CreateProblemService(testData);

            int id = 2;
            string problemName = service.GetProblemName(id);
            string expectedName = testData[id - 1].Name;

            Assert.Equal(expectedName, problemName);
        }

        [Fact]
        public async Task GetProblemName_WithInValidId_ShouldReturnNull()
        {
            List<Problem> testData = GetTestData();
            ProblemService service = await CreateProblemService(testData);

            Assert.Null(service.GetProblemName(321));
        }

        private async Task<ProblemService> CreateProblemService(List<Problem> testData)
        {
            await context.Problems.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IDeletableEntityRepository<Problem> repository = new EfDeletableEntityRepository<Problem>(context);
            var service = new ProblemService(repository);
            return service;
        }

        private async Task AddData(IEnumerable<Problem> testData)
        {
            await context.Problems.AddRangeAsync(testData);
            await context.SaveChangesAsync();
        }

        private ProblemService CreateProblemServiceWithMockedRepository(IQueryable<Problem> testData)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<Problem>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new ProblemService(reposotiryMock.Object);
        }

        private List<Problem> GetTestData()
        {
            var problems = new List<Problem>
            {
                new Problem { Id = 1, Name = "test1", LessonId = 1, IsExtraTask = false, MaxPoints = 100 },
                new Problem { Id = 2, Name = "test2", LessonId = 2, IsExtraTask = false, MaxPoints = 50 },
                new Problem { Id = 3, Name = "test3", LessonId = 3, IsExtraTask = true, MaxPoints = 100 },
                new Problem { Id = 4, Name = "test4", LessonId = 2, IsExtraTask = false, MaxPoints = 200 },
            };
            return problems;
        }
    }
}
