using JudgeSystem.Services.Mapping;
using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.ViewModels.Problem
{
	public class ContestProblemViewModel : IMapFrom<Data.Models.Problem>
	{
		public int Id { get; set; }

		public string Name { get; set; }

        public int MaxPoints { get; set; }

        public bool IsExtraTask { get; set; }
    }
}
