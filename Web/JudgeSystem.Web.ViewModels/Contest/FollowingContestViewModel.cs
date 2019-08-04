using JudgeSystem.Services.Mapping;
using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.ViewModels.Contest
{
	public class ContestBreifInfoViewModel : IMapFrom<Data.Models.Contest>
	{
		public string Name { get; set; }

		public int Id { get; set; }
	}
}
