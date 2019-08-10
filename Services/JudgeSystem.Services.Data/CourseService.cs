using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Course;
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
			await this.repository.AddAsync(course);
			await this.repository.SaveChangesAsync();
		}

		public IEnumerable<CourseViewModel> All()
		{
			return this.repository.All().To<CourseViewModel>().ToList();
		}

		public string GetName(int courseId)
		{
			return repository.All().FirstOrDefault(r => r.Id == courseId)?.Name;
		}

		public TDestination GetById<TDestination>(int courseId)
		{
            var course = repository.All().Where(x => x.Id == courseId).To<TDestination>().FirstOrDefault();
            if (course == null)
            {
                throw new EntityNotFoundException("course");
            }
            return course;
        }

        public async Task Updade(CourseEditModel model)
		{
            var course = await repository.FindAsync(model.Id);
            course.Name = model.Name;

            repository.Update(course);
			await repository.SaveChangesAsync();
		}

		public async Task<CourseViewModel> Delete(int id)
		{
            var course = await repository.FindAsync(id);

            repository.Delete(course);
			await repository.SaveChangesAsync();

            return course.To<CourseViewModel>();
		}
	}
}
