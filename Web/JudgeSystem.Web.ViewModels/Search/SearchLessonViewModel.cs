namespace JudgeSystem.Web.ViewModels.Search
{
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Data.Models;

    public class SearchLessonViewModel : IMapFrom<Lesson>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
