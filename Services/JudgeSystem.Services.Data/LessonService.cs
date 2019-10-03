using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Lesson;
using JudgeSystem.Web.InputModels.Lesson;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.ViewModels.Search;
using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class LessonService : ILessonService
    {
        private readonly IDeletableEntityRepository<Lesson> repository;
        private readonly IPasswordHashService hashService;

        public LessonService(IDeletableEntityRepository<Lesson> repository, IPasswordHashService hashService)
        {
            this.repository = repository;
            this.hashService = hashService;
        }

        public IEnumerable<LessonLinkViewModel> CourseLessonsByType(string lessonType, int courseId)
        {
            bool isValidLessonType = Enum.TryParse(lessonType, out LessonType type);
            if (!isValidLessonType)
            {
                return Enumerable.Empty<LessonLinkViewModel>();
            }

            return repository.All()
                .Where(l => l.Type == type && l.CourseId == courseId)
                .To<LessonLinkViewModel>()
                .ToList();
        }

        public async Task<LessonDto> Create(LessonInputModel model)
        {
            Lesson lesson = model.To<Lesson>();
            if (!string.IsNullOrEmpty(lesson.LessonPassword))
            {
                lesson.LessonPassword = hashService.HashPassword(lesson.LessonPassword);
            }

            await repository.AddAsync(lesson);
            return lesson.To<LessonDto>();
        }

        public async Task<LessonDto> Delete(int id)
        {
            Lesson lesson = await repository.FindAsync(id);
            await repository.DeleteAsync(lesson);
            return lesson.To<LessonDto>();
        }

        public async Task<TDestination> GetById<TDestination>(int id)
        {
            TDestination lesson = await repository.All().Where(x => x.Id == id).To<TDestination>().FirstOrDefaultAsync();
            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(lesson, nameof(Lesson));
            return lesson;
        }

        public IEnumerable<ContestLessonDto> GetCourseLesosns(int courseId, LessonType lesosnType)
        {
            var lessons = repository.All()
                .Where(l => l.CourseId == courseId && l.Type == lesosnType)
                .To<ContestLessonDto>()
                .ToList();

            return lessons;
        }

        public int? GetFirstProblemId(int lessonId)
        {
            ThrowEntityNotFoundExceptionIfLessonDoesNotExist(lessonId);

            return repository.All()
                .Include(x => x.Problems)
                .First(x => x.Id == lessonId).Problems
                .OrderBy(x => x.CreatedOn).FirstOrDefault()?.Id;
        }
        public async Task<LessonViewModel> GetLessonInfo(int id)
        {
            LessonViewModel lesson = await repository.All()
                .Where(x => x.Id == id)
                .To<LessonViewModel>()
                .FirstOrDefaultAsync();

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(lesson, nameof(Lesson));
            return lesson;
        }

        public int GetPracticeId(int lessonId)
        {
            ThrowEntityNotFoundExceptionIfLessonDoesNotExist(lessonId);

            return repository.All()
                .Where(x => x.Id == lessonId)
                .Select(x => x.Practice.Id)
                .First();
        }

        public IEnumerable<SearchLessonViewModel> SearchByName(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new BadRequestException(ErrorMessages.InvalidSearchKeyword);
            }

            keyword = keyword.ToLower();
            var results = repository.All()
                .Where(l => l.Name.ToLower().Contains(keyword))
                .To<SearchLessonViewModel>()
                .ToList();

            return results;
        }

        public async Task SetPassword(int id, string lessonPassword)
        {
            Lesson lesson = await repository.FindAsync(id);
            if (!string.IsNullOrEmpty(lesson.LessonPassword))
            {
                throw new BadRequestException(ErrorMessages.LockedLesson);
            }

            lesson.LessonPassword = hashService.HashPassword(lessonPassword);
            await repository.UpdateAsync(lesson);
        }

        public async Task<LessonDto> Update(LessonEditInputModel model)
        {
            Lesson lesson = await repository.FindAsync(model.Id);
            lesson.Name = model.Name;
            lesson.Type = model.Type;

            await repository.UpdateAsync(lesson);
            return lesson.To<LessonDto>();
        }

        public async Task<LessonDto> UpdatePassword(int lessonId, string oldPassword, string newPassword)
        {
            Lesson lesson = await repository.FindAsync(lessonId);
            if (lesson.LessonPassword != hashService.HashPassword(oldPassword))
            {
                throw new BadRequestException(ErrorMessages.DiffrentLessonPasswords);
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                lesson.LessonPassword = null;
            }
            else
            {
                lesson.LessonPassword = hashService.HashPassword(newPassword);
            }

            await repository.UpdateAsync(lesson);
            return lesson.To<LessonDto>();
        }

        private bool Exists(int id) => repository.All().Any(x => x.Id == id);

        private void ThrowEntityNotFoundExceptionIfLessonDoesNotExist(int lessonId)
        {
            if (!Exists(lessonId))
            {
                throw new EntityNotFoundException(nameof(Lesson));
            }
        }
    }
}
