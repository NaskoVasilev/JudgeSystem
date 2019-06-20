namespace JudgeSystem.Services.Data
{
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
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

		public Task<Student> GetStudentProfileByActivationKey(string activationKey)
		{
			string activationKeyHash = passwordHashService.HashPassword(activationKey);
			return repository.All().FirstOrDefaultAsync(s => s.ActivationKeyHash == activationKeyHash);
		}

		public async Task SetStudentProfileAsActivated(Student student)
		{
			student.IsActivated = true;
			repository.Update(student);
			await repository.SaveChangesAsync();
		}
	}
}
