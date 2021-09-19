using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.ViewModels.Course;

namespace JudgeSystem.Services.Data
{
    public class CourseService : ICourseService
	{
		private readonly IDeletableEntityRepository<Course> repository;

		public CourseService(IDeletableEntityRepository<Course> repository)
		{
			this.repository = repository;
		}

		public async Task Add(CourseInputModel model)
		{
			Course course = model.To<Course>();
			await repository.AddAsync(course);
		}

        public IEnumerable<CourseViewModel> All() => repository
            .All()
            .OrderBy(x => x.OrderBy)
            .To<CourseViewModel>()
            .ToList();

        public string GetName(int courseId) => repository
            .All()
            .Where(x => x.Id == courseId)
            .Select(x => x.Name)
            .FirstOrDefault();

        public TDestination GetById<TDestination>(int courseId)
		{
            TDestination course = repository
                .All()
                .Where(x => x.Id == courseId)
                .To<TDestination>()
                .FirstOrDefault();

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(course, nameof(Course));
            return course;
        }

        public async Task Updade(CourseEditModel model)
		{
            Course course = await repository.FindAsync(model.Id);
            course.Name = model.Name;
            course.OrderBy = model.OrderBy;

            await repository.UpdateAsync(course);
		}

		public async Task<CourseViewModel> Delete(int id)
		{
            Course course = await repository.FindAsync(id);
            await repository.DeleteAsync(course);
            return course.To<CourseViewModel>();
		}
	}
}
