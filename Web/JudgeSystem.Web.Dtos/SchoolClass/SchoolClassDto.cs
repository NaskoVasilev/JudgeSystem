using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.SchoolClass
{
    public class SchoolClassDto : IMapFrom<Data.Models.SchoolClass>
	{
		public int Id { get; set; }

		public int ClassNumber { get; set; }

		public SchoolClassType ClassType { get; set; }

        public string Name { get; set; }
    }
}
