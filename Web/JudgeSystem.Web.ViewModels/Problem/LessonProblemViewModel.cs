using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.Problem
{
    public class LessonProblemViewModel : IMapFrom<Data.Models.Problem>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
