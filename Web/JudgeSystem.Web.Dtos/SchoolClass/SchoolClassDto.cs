namespace JudgeSystem.Web.Dtos.SchoolClass
{
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class SchoolClassDto : IMapFrom<SchoolClass>
	{
		public int Id { get; set; }

		public int ClassNumber { get; set; }

		public SchoolClassType ClassType { get; set; }
	}
}
