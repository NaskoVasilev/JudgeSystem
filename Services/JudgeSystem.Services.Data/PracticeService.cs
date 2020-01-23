using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.ViewModels.Practice;
using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Common.Extensions;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class PracticeService : IPracticeService
    {
        private readonly IDeletableEntityRepository<Practice> repository;
        private readonly IRepository<UserPractice> userPracticeRepository;
        private readonly IPaginationService paginationService;

        public PracticeService(
            IDeletableEntityRepository<Practice> repository, 
            IRepository<UserPractice> userPracticeRepository,
            IPaginationService paginationService)
        {
            this.repository = repository;
            this.userPracticeRepository = userPracticeRepository;
            this.paginationService = paginationService;
        }

        public async Task AddUserToPracticeIfNotAdded(string userId, int practiceId)
        {
            if (!userPracticeRepository.All().Any(x => x.UserId == userId && x.PracticeId == practiceId))
            {
                await userPracticeRepository.AddAsync(new UserPractice { PracticeId = practiceId, UserId = userId });
            }
        }

        public async Task<int> Create(int lessonId)
        {
            var practice = new Practice { LessonId = lessonId };
            await repository.AddAsync(practice);
            return practice.Id;
        }

        public async Task<LessonDto> GetLesson(int practiceId)
        {
            Practice practice = await repository.All()
                .Include(x => x.Lesson)
                .FirstOrDefaultAsync(x => x.Id == practiceId);

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(practice, nameof(Practice));

            return practice.Lesson.To<LessonDto>();
        }

        public PracticeAllResultsViewModel GetPracticeResults(int id, int page, int entitesPerPage)
        {
            PracticeAllResultsViewModel model = repository.All()
                .Where(practice => practice.Id == id)
                .Select(practice => new PracticeAllResultsViewModel()
                {
                    Id = practice.Id,
                    LessonName = practice.Lesson.Name,
                    Problems = practice.Lesson.Problems
                    .OrderBy(problem => problem.CreatedOn)
                    .Select(problem => new PracticeProblemViewModel
                    {
                        Id = problem.Id,
                        Name = problem.Name,
                        IsExtraTask = problem.IsExtraTask,
                        MaxPoints = problem.MaxPoints
                    })
                    .ToList(),
                    PracticeResults = practice.UserPractices
                    .Select(userPractice => new PracticeResultViewModel
                    {
                        UserId = userPractice.User.Id,
                        Username = userPractice.User.UserName,
                        FullName = $"{userPractice.User.Name} {userPractice.User.Surname}",
                        PointsByProblem = userPractice.User.Submissions
                        .Where(submission => submission.PracticeId == practice.Id)
                        .GroupBy(submission => submission.ProblemId)
                        .ToDictionary(problemBySubmissions => 
                            problemBySubmissions.Key, 
                            submissions => submissions.Max(s => s.ActualPoints))
                    })
                    .OrderByDescending(cr => cr.Total)
                    .ThenBy(cr => cr.Username)
                    .GetPage(page, entitesPerPage)
                    .ToList(),
                })
                .FirstOrDefault();

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(model, nameof(Contest));

            return model;
        }

        public int GetPracticeResultsPagesCount(int id, int entitesPerPage)
        {
            int userPracticesCount = userPracticeRepository
                .All()
                .Count(userPractice => userPractice.PracticeId == id);
            
            return paginationService.CalculatePagesCount(userPracticesCount, entitesPerPage);
        }
    }
}
