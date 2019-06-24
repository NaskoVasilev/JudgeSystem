using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

		public IActionResult Results([Required, MinLength(GlobalConstants.SearchKeywordMinLength)] string keyword)
		{
			if(!ModelState.IsValid)
			{
				return View("ErrorView");
			}

			SearchResultsViewModel searchResults = new SearchResultsViewModel();
			searchResults.Problems = problemService.SerchByName(keyword).ToList();
			searchResults.Lessons = lessonService.SearchByName(keyword).ToList();
			return this.View(searchResults);
		}
	}
}
