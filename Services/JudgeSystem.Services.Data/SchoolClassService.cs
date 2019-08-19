using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.SchoolClass;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class SchoolClassService : ISchoolClassService
	{
		private readonly IDeletableEntityRepository<SchoolClass> repository;

		public SchoolClassService(IDeletableEntityRepository<SchoolClass> repository)
		{
			this.repository = repository;
		}

        public bool ClassExists(int classNumber, SchoolClassType classType) => 
            repository.All().Any(c => c.ClassNumber == classNumber && c.ClassType == classType);

        public async Task<SchoolClassDto> Create(int classNumber, SchoolClassType classType)
		{
			var schoolClass = new SchoolClass { ClassNumber = classNumber, ClassType = classType };
			await repository.AddAsync(schoolClass);
			return schoolClass.To<SchoolClassDto>();
		}

		public IEnumerable<SchoolClassDto> GetAllClasses()
		{
			var classes = repository.All()
                .OrderBy(x => x.ClassNumber)
                .ThenBy(x => x.ClassType)
				.To<SchoolClassDto>()
				.ToList();
            return classes;
		}

        public async Task<T> GetById<T>(int id)
        {
            T schoolClass = await repository.All().Where(x => x.Id == id).To<T>().FirstOrDefaultAsync();
            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(schoolClass, nameof(SchoolClass));
            return schoolClass;
        }
    }
}
