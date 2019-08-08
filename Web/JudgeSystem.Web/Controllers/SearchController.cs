using System.Linq;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Web.ViewModels.Search;
using JudgeSystem.Common;
using JudgeSystem.Services.Data;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
	public class SearchController : BaseController
	{
		private readonly IProblemService problemService;
		private readonly ILessonService lessonService;

		public SearchController(IProblemService problemService, ILessonService lessonService)
		{
			this.problemService = problemService;
			this.lessonService = lessonService;
		}

		public IActionResult Results([Required] string keyword)
		{
			if(!ModelState.IsValid)
			{
				return View("ErrorView");
			}

            SearchResultsViewModel searchResults = new SearchResultsViewModel
            {
                Problems = problemService.SearchByName(keyword).ToList(),
                Lessons = lessonService.SearchByName(keyword).ToList()
            };

            return this.View(searchResults);
		}
	}
}
