using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.Search
{
    public class SearchLessonViewModel : IMapFrom<Data.Models.Lesson>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
