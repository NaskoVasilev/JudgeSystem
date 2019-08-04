namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Mapping;
    using JudgeSystem.Web.Dtos.Student;
    using JudgeSystem.Web.Infrastructure.Exceptions;
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

		public async Task<StudentDto> Create(StudentCreateInputModel model, string activationKey)
		{
            var student = model.To<Student>();
			student.ActivationKeyHash = passwordHashService.HashPassword(activationKey);
			await repository.AddAsync(student);
			await repository.SaveChangesAsync();
			return student.To<StudentDto>();
		}

		public async Task Delete(string id)
		{
            var student = await repository.All().FirstOrDefaultAsync(x => x.Id == id);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }

            this.repository.Delete(student);
			await this.repository.SaveChangesAsync();
		}

		public async Task<T> GetById<T>(string id)
		{
            var student = await repository.All().Where(s => s.Id == id).To<T>().FirstOrDefaultAsync();
            if(student == null)
            {
                throw new EntityNotFoundException();
            }
			return student;
		}

		public async Task<SchoolClassDto> GetStudentClass(string id)
		{
			Student student = await this.repository.All()
                .Include(s => s.SchoolClass)
                .FirstOrDefaultAsync(s => s.Id == id);

            if(student == null)
            {
                throw new EntityNotFoundException();
            }

            return student.SchoolClass.To<SchoolClassDto>();
		}

		public async Task<StudentProfileViewModel> GetStudentInfo(string studentId)
		{
            return await this.GetById<StudentProfileViewModel>(studentId);
		}

		public Task<StudentDto> GetStudentProfileByActivationKey(string activationKey)
		{
			string activationKeyHash = passwordHashService.HashPassword(activationKey);
			return repository.All()
                .Where(x => x.ActivationKeyHash == activationKeyHash)
                .To<StudentDto>()
                .FirstOrDefaultAsync();
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
				students = repository.All()
					.Where(s => s.SchoolClass.ClassType == classType.Value);
			}
			else
			{
				return new List<StudentProfileViewModel>();
			}
			
			return students
                .Where(s => s.IsActivated)
				.OrderBy(s => s.SchoolClass.ClassNumber)
				.ThenBy(s => s.SchoolClass.ClassType)
				.ThenBy(s => s.NumberInCalss)
				.To<StudentProfileViewModel>()
                .ToList();
		}

		public async Task SetStudentProfileAsActivated(string studentId)
		{
            var student = await repository.All().FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }

            student.IsActivated = true;
			repository.Update(student);
			await repository.SaveChangesAsync();
		}

		public async Task<StudentDto> Update(StudentEditInputModel model)
		{
            if (!this.Exists(model.Id))
            {
                throw new EntityNotFoundException();
            }

            Student student = await repository.All().FirstOrDefaultAsync(s => s.Id == model.Id);
            if(student == null)
            {
                throw new EntityNotFoundException(nameof(student));
            }

			student.FullName = model.FullName;
			student.Email = model.Email;
			student.NumberInCalss = model.NumberInCalss;
			student.SchoolClassId = model.SchoolClassId;
			repository.Update(student);
			await repository.SaveChangesAsync();

			return student.To<StudentDto>();
		}

        private bool Exists(string id)
        {
            return this.repository.All().Any(x => x.Id == id);
        }
    }
}
