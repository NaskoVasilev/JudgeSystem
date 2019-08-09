using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Dtos.SchoolClass;

namespace JudgeSystem.Services.Data
{
	public interface ISchoolClassService
	{
		IEnumerable<SchoolClassDto> GetAllClasses();

		bool ClassExists(int classNumber, SchoolClassType classType);

		Task<SchoolClassDto> Create(int classNumber, SchoolClassType classType);

        Task<T> GetById<T>(int id);
 	}
}
