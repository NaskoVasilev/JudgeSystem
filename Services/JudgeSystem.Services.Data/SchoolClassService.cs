namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Linq;
    using System.Threading.Tasks;
    using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.Dtos.SchoolClass;

	public class SchoolClassService : ISchoolClassService
	{
		private readonly IDeletableEntityRepository<SchoolClass> repository;

		public SchoolClassService(IDeletableEntityRepository<SchoolClass> repository)
		{
			this.repository = repository;
		}

		public bool ClassExists(int classNumber, SchoolClassType classType)
		{
			return this.repository.All().Any(c => c.ClassNumber == classNumber && c.ClassType == classType);
		}

		public async Task<SchoolClass> Create(int classNumber, SchoolClassType classType)
		{
			SchoolClass schoolClass = new SchoolClass { ClassNumber = classNumber, ClassType = classType };
			await repository.AddAsync(schoolClass);
			await repository.SaveChangesAsync();
			return schoolClass;
		}

		public IEnumerable<SchoolClassDto> GetAllClasses()
		{
			return repository.All()
				.To<SchoolClassDto>()
				.ToList();
		}
	}
}
