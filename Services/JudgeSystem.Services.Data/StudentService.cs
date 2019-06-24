namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JudgeSystem.Common;
    using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.ViewModels.Student;

	using Microsoft.EntityFrameworkCore;

	public class StudentService : IStudentService
	{
		private readonly IPasswordHashService passwordHashService;
		private readonly IRepository<Student> repository;

		public StudentService(IPasswordHashService passwordHashService, IRepository<Student> repository)
		{
			this.passwordHashService = passwordHashService;
			this.repository = repository;
		}

		public async Task<Student> Create(Student student)
		{
			student.ActivationKeyHash = passwordHashService.HashPassword(student.ActivationKeyHash);
			await repository.AddAsync(student);
			await repository.SaveChangesAsync();
			return student;
		}

		public async Task<StudentProfileViewModel> GetStudentInfo(string studentId)
		{
			var student = await this.repository.All().Include(s => s.SchoolClass).FirstOrDefaultAsync(s => s.Id == studentId);
			return student.To<StudentProfileViewModel>();
		}

		public Task<Student> GetStudentProfileByActivationKey(string activationKey)
		{
			string activationKeyHash = passwordHashService.HashPassword(activationKey);
			return repository.All().FirstOrDefaultAsync(s => s.ActivationKeyHash == activationKeyHash);
		}

		public IEnumerable<StudentProfileViewModel> SearchStudentsByClass(int? classNumber, SchoolClassType? classType)
		{
			IQueryable<Student> students = null;

			if (classNumber.HasValue && classType.HasValue)
			{
				students = repository.All()
					.Where(s => s.SchoolClass.ClassNumber == classNumber.Value && s.SchoolClass.ClassType == classType.Value);
			}
			else if(classNumber.HasValue)
			{
				students = repository.All()
					.Where(s => s.SchoolClass.ClassNumber == classNumber.Value);
			}
			else if(classType.HasValue)
			{
				SchoolClassType schoolClassType = (SchoolClassType)classType;
				students = repository.All()
					.Where(s => s.SchoolClass.ClassType == classType.Value);
			}
			else
			{
				return new List<StudentProfileViewModel>();
			}
			
			return students
				.OrderBy(s => s.SchoolClass.ClassNumber)
				.ThenBy(s => s.SchoolClass.ClassType)
				.ThenBy(s => s.SchoolClass)
				.To<StudentProfileViewModel>().ToList();
		}

		public async Task SetStudentProfileAsActivated(Student student)
		{
			student.IsActivated = true;
			repository.Update(student);
			await repository.SaveChangesAsync();
		}
	}
}
