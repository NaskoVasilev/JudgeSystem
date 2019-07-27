using System.Linq;
using System.Threading.Tasks;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;

namespace JudgeSystem.Services.Data
{
    public class PracticeService : IPracticeService
    {
        private readonly IDeletableEntityRepository<Practice> repository;
        private readonly IRepository<UserPractice> userPracticeRepository;

        public PracticeService(IDeletableEntityRepository<Practice> repository, IRepository<UserPractice> userPracticeRepository)
        {
            this.repository = repository;
            this.userPracticeRepository = userPracticeRepository;
        }

        public async Task AddUserToPracticeIfNotAdded(string userId, int practiceId)
        {
            if (!userPracticeRepository.All().Any(x => x.UserId == userId && x.PracticeId == practiceId))
            {
                await userPracticeRepository.AddAsync(new UserPractice { PracticeId = practiceId, UserId = userId });
                await userPracticeRepository.SaveChangesAsync();
            }
        }

        public async Task<Practice> Create(int lessonId)
        {
            Practice practice = new Practice { LessonId = lessonId };
            await repository.AddAsync(practice);
            await repository.SaveChangesAsync();

            return practice;
        }
    }
}
