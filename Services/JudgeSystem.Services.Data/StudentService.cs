namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.InputModels.Student;
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

		public async Task DeleteAsync(Student student)
		{
			this.repository.Delete(student);
			await this.repository.SaveChangesAsync();
		}

		public async Task<T> GetById<T>(string id)
		{
			var student = await repository.All()
				.Where(s => s.Id == id)
				.To<T>()
				.FirstOrDefaultAsync();

			return student;
		}

		public async Task<Student> GetById(string id)
		{
			return await repository.All().FirstOrDefaultAsync(s => s.Id == id);
		}

		public async Task<SchoolClass> GetStudentClassAsync(string id)
		{
			Student student = await this.repository.All().Include(s => s.SchoolClass).FirstOrDefaultAsync(s => s.Id == id);
			return student.SchoolClass;
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

		public async Task<Student> UpdateAsync(StudentEditInputModel model)
		{
			Student student = await repository.All().FirstOrDefaultAsync(s => s.Id == model.Id);
			student.FullName = model.FullName;
			student.Email = model.Email;
			student.NumberInCalss = model.NumberInCalss;
			student.SchoolClassId = model.SchoolClassId;
			repository.Update(student);
			await repository.SaveChangesAsync();
			return student;
		}
	}
}
