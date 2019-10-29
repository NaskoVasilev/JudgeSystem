using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.Submission;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Services;
using JudgeSystem.Web.ViewModels.Submission;
using JudgeSystem.Common;
using JudgeSystem.Web.Filters;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
    public class SubmissionController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISubmissionService submissionService;
        private readonly IUtilityService utilityService;

        public SubmissionController(
            UserManager<ApplicationUser> userManager,
            ISubmissionService submissionService,
            IUtilityService utilityService)
        {
            this.userManager = userManager;
            this.submissionService = submissionService;
            this.utilityService = utilityService;
        }

        public IActionResult Details(int id)
        {
            SubmissionViewModel submission = submissionService.GetSubmissionDetails(id);
            return View(submission);
        }

        public IActionResult Download(int id)
        {
            byte[] submissionCode = submissionService.GetSubmissionCodeById(id);
            string problemName = submissionService.GetProblemNameBySubmissionId(id);

            return File(submissionCode, GlobalConstants.OctetStreamMimeType, $"{problemName}.zip");
        }

        [EndpointExceptionFilter]
        public IActionResult GetProblemSubmissions(int problemId, int page = 1,
            int submissionsPerPage = GlobalConstants.SubmissionsPerPage, int? contestId = null)
        {
            string userId = userManager.GetUserId(User);
            IEnumerable<SubmissionResult> submissionResults = new List<SubmissionResult>();
            if (contestId.HasValue)
            {
                submissionResults = submissionService.GetUserSubmissionsByProblemIdAndContestId(contestId.Value, problemId, userId, page, submissionsPerPage);
            }
            else
            {
                submissionResults = submissionService
                .GetUserSubmissionsByProblemId(problemId, userId, page, submissionsPerPage);
            }

            return Json(submissionResults);
        }

        [EndpointExceptionFilter]
        public IActionResult GetSubmissionsCount(int problemId, int? contestId = null)
        {
            string userId = userManager.GetUserId(User);
            int submissionsCount = 0;

            if (contestId.HasValue)
            {
                submissionsCount = submissionService.GetSubmissionsCountByProblemIdAndContestId(problemId, contestId.Value, userId);
            }
            else
            {
                submissionsCount = submissionService.GetProblemSubmissionsCount(problemId, userId);
            }
            return Json(submissionsCount);
        }

        //[EndpointExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> Create(SubmissionInputModel model)
        {
            SubmissionCodeDto submissionCode = await utilityService.ExtractSubmissionCode(model.Code, model.File, model.ProgrammingLanguage);

            string userId = userManager.GetUserId(User);
            model.SubmissionContent = submissionCode.Content;
            SubmissionDto submission = await submissionService.Create(model, userId);

            await submissionService.ExecuteSubmission(submission.Id, submissionCode.SourceCodes, model.ProgrammingLanguage);
            await submissionService.CalculateActualPoints(submission.Id);

            SubmissionResult submissionResult = submissionService.GetSubmissionResult(submission.Id);
            return Json(submissionResult);
        }
    }
}
