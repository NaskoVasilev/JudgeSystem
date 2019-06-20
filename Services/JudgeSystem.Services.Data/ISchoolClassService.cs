namespace JudgeSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Data.Models.Enums;
    using JudgeSystem.Web.Dtos.SchoolClass;

	public interface ISchoolClassService
	{
		IEnumerable<SchoolClassDto> GetAllClasses();

		bool ClassExists(int classNumber, SchoolClassType classType);

		Task<SchoolClass> Create(int classNumber, SchoolClassType classType);
	}
}
