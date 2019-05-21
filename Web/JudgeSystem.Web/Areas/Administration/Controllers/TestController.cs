using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Services.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	public class TestController : AdministrationBaseController
	{
		private readonly ITestService testService;

		public TestController(ITestService testService)
		{
			this.testService = testService;
		}

		public IActionResult ProblemTasks(int problemId)
		{
			var tests = testService.TestsByProblem(problemId);
			//TODO: Create View Problem Tests and create partial view for a test
			return View(tests);
		}

		public async Task<IActionResult> Edit(int id)
		{
			Test test = await testService.GetById(id);
			if(test == null)
			{
				//TODO: need to pass problemId
				string errorMessage = string.Format(ErrorMessages.NotFoundEntityMessage, "test");
				this.ShowError(errorMessage, nameof(ProblemTasks), "Test");
			}

			//var model = test.To<Test, TestEditInputModel>();
			return View();
		}

		[HttpPost]
		public IActionResult Edit()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			return View();
		}
	}
}
