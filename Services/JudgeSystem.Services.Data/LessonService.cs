using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Lesson;
using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
	public class LessonService : ILessonService
	{
		private readonly IDeletableEntityRepository<Lesson> repository;

		public LessonService(IDeletableEntityRepository<Lesson> repository)
		{
			this.repository = repository;
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

		public async Task<Lesson> CreateLesson(LessonInputModel model, IEnumerable<Resource> resources)
		{
			Lesson lesson = model.To<LessonInputModel, Lesson>();
			lesson.Resources = resources.ToList();
			await repository.AddAsync(lesson);
			await repository.SaveChangesAsync();
			return lesson;
		}

		public async Task Delete(Lesson lesson)
		{
			this.repository.Delete(lesson);
			await this.repository.SaveChangesAsync();
		}

		public async Task<Lesson> GetById(int id)
		{
			return await repository.All()
				.FirstOrDefaultAsync(l => l.Id == id);
		}

		public async Task<LessonViewModel> GetLessonInfo(int id)
		{
			var lesson =  await this.repository.All()
				.Include(l => l.Problems)
				.Include(l => l.Resources)
				.FirstOrDefaultAsync(l => l.Id == id);
			return lesson.To<Lesson, LessonViewModel>();
		}

		public async Task Update(Lesson lesson)
		{
			repository.Update(lesson);
			await repository.SaveChangesAsync();
		}
	}
}
