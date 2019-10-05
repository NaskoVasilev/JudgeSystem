using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Lesson;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class PracticeService : IPracticeService
    {
        private readonly IDeletableEntityRepository<Practice> repository;
        private readonly IRepository<UserPractice> userPracticeRepository;

        public PracticeService(
            IDeletableEntityRepository<Practice> repository, 
            IRepository<UserPractice> userPracticeRepository)
        {
            this.repository = repository;
            this.userPracticeRepository = userPracticeRepository;
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
    }
}
