namespace JudgeSystem.Services.Data
{
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;

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
	}
}
