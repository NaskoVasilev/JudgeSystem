using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.InputModels.Problem;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Web.Utilites;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Web.Utilites.ImportTests;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class ProblemController : AdministrationBaseController
    {
        private readonly IProblemService problemService;
        private readonly ITestService testService;
        private readonly ILessonService lessonService;
        private readonly ISubmissionService submissionService;
        private readonly IServiceProvider serviceProvider;

        public ProblemController(
            IProblemService problemService,
            ITestService testService,
            ILessonService lessonService,
            ISubmissionService submissionService,
            IServiceProvider serviceProvider)
        {
            this.problemService = problemService;
            this.testService = testService;
            this.lessonService = lessonService;
            this.submissionService = submissionService;
            this.serviceProvider = serviceProvider;
        }

        public IActionResult Create() => View(new ProblemInputModel());

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ProblemInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ProblemDto problem = await problemService.Create(model);
            return RedirectToAction(nameof(AddTest), "Problem",
                new { area = GlobalConstants.AdministrationArea, problemId = problem.Id });
        }

        public IActionResult All(int lessonId)
        {
            IEnumerable<LessonProblemViewModel> problems = problemService.LessonProblems(lessonId);
            int practiceId = lessonService.GetPracticeId(lessonId);
            var model = new ProblemAllViewModel
            {
                Problems = problems,
                LesosnId = lessonId,
                PracticeId = practiceId
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ProblemEditInputModel model = await problemService.GetById<ProblemEditInputModel>(id);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(ProblemEditInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ProblemDto problem = await problemService.Update(model);
            return RedirectToAction(nameof(All), "Problem",
                new { area = GlobalConstants.AdministrationArea, problem.LessonId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            ProblemViewModel model = await problemService.GetById<ProblemViewModel>(id);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeletePost(int id)
        {
            ProblemDto problem = await problemService.Delete(id);
            //Delete all submissions for current problem becuase when we make reports - submission for some deleted problems 
            //exists in the database and they are also included in the report results which is incorrect behaviour
            await submissionService.DeleteSubmissionsByProblemId(id);

            return RedirectToAction(nameof(All), "Problem",
                new { area = GlobalConstants.AdministrationArea, problem.LessonId });
        }

        public async Task<IActionResult> AddTest(int problemId)
        {
            string problemName = problemService.GetProblemName(problemId);
            var model = new TestInputModel { LessonId = await problemService.GetLessonId(problemId), ProblemName = problemName };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddTest(TestInputModel model)
        {
            if (!ModelState.IsValid)
            {
                model.LessonId = await problemService.GetLessonId(model.ProblemId);
                return View(model);
            }

            await testService.Add(model);

            string problemName = problemService.GetProblemName(model.ProblemId);
            string infoMessage = string.Format(InfoMessages.AddedTest, problemName);
            return ShowInfo(infoMessage, nameof(AddTest), new { problemId = model.ProblemId });
        }

        public async Task<IActionResult> AddTests(int problemId)
        {
            string problemName = problemService.GetProblemName(problemId);
            int lessonId = await problemService.GetLessonId(problemId);
            var model = new ProblemAddTestsInputModel { ProblemName = problemName, LessonId = lessonId };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddTests(ProblemAddTestsInputModel model)
        {
            model.LessonId = await problemService.GetLessonId(model.ProblemId);
            model.ProblemName = problemService.GetProblemName(model.ProblemId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Tests.FileName) || !model.Tests.FileName.EndsWith(GlobalConstants.JsonFileExtension))
            {
                ModelState.AddModelError(nameof(ProblemAddTestsInputModel.Tests), ErrorMessages.FileExtensionMustBeJson);
                return View(model);
            }

            var errorMessages = new List<string>();
            ParseTestsStrategy parseTestsStrategy = new ParseTestsStartegyFactory().CreateStrategy(model.Strategy);
            IEnumerable<ProblemTestInputModel> tests = parseTestsStrategy.Parse(serviceProvider, model.Tests, errorMessages);

            if (errorMessages.Any())
            {
                foreach (string errorMessage in errorMessages)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
                return View(model);
            }

            ValidateTests(tests.ToList());

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await testService.AddRange(tests, model.ProblemId);

            return RedirectToAction(nameof(TestController.ProblemTests), nameof(TestController).ToControllerName(), new { model.ProblemId });
        }

        private void ValidateTests(List<ProblemTestInputModel> tests)
        {
            for (int i = 0; i < tests.Count; i++)
            {
                IEnumerable<string> errors = Utility.ValidateObject(tests[i]);
                if (errors.Any())
                {
                    string errorMessage = $"{string.Format(ErrorMessages.InvalidTest, i + 1)}{Environment.NewLine}{string.Join(Environment.NewLine, errors)}";
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }
        }
    }
}
