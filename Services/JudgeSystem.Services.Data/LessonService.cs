using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Lesson;

namespace JudgeSystem.Services.Data
{
	public class LessonService : ILessonService
	{
		private readonly IDeletableEntityRepository<Lesson> repository;

		public LessonService(IDeletableEntityRepository<Lesson> repository)
		{
			this.repository = repository;
		}

		public IEnumerable<LessonLinkViewModel> LessonsByType(string lessonType)
		{
			bool isValidLessonType = Enum.TryParse<LessonType>(lessonType, out LessonType type);
			if (isValidLessonType)
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
