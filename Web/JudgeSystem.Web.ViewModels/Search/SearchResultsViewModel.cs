using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Search
{
	public class SearchResultsViewModel
	{
		public List<SearchProblemViewModel> Problems { get; set; }

		public List<SearchLessonViewModel> Lessons { get; set; }
	}
}
