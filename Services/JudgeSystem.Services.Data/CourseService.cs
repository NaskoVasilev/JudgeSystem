using AutoMapper;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Course;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			Course course = model.To<CourseInputModel, Course>();
			await this.repository.AddAsync(course);
			await this.repository.SaveChangesAsync();
		}

		public IEnumerable<CourseViewModel> All()
		{
			return this.repository.All().To<CourseViewModel>().ToList();
		}
	}
}
