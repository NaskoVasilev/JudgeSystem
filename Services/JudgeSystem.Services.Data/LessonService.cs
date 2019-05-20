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

		public async Task CreateLesson(LessonInputModel model, IEnumerable<Resource> resources)
		{
			Lesson lesson = model.To<LessonInputModel, Lesson>();
			lesson.Resources = resources.ToList();
			await repository.AddAsync(lesson);
			await repository.SaveChangesAsync();
		}

		public async Task<LessonViewModel> GetLessonInfo(int id)
		{
			var lesson =  await this.repository.All()
				.Include(l => l.Problems)
				.Include(l => l.Resources)
				.FirstOrDefaultAsync(l => l.Id == id);
			return lesson.To<Lesson, LessonViewModel>();
		}

		public IEnumerable<LessonLinkViewModel> LessonsByType(string lessonType)
		{
			bool isValidLessonType = Enum.TryParse(lessonType, out LessonType type);
			if (!isValidLessonType)
			{
				return Enumerable.Empty<LessonLinkViewModel>();
			}

			return repository.All()
				.Where(l => l.Type == type)
				.To<LessonLinkViewModel>()
				.ToList();
		}
	}
}
