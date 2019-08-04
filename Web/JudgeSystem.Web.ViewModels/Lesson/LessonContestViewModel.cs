using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.Lesson
{
    public class LessonContestViewModel : IMapFrom<Data.Models.Contest>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
