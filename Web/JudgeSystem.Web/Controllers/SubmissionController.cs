using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.Submission;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Services;
using JudgeSystem.Web.ViewModels.Submission;
using JudgeSystem.Common;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Web.Filters;
using JudgeSystem.Data.Models.Enums;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
    public class SubmissionController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISubmissionService submissionService;
        private readonly IProblemService problemService;
        private readonly IUtilityService utilityService;
        private readonly IContestService contestService;
        private readonly ICodeCompareer codeCompareer;
        private readonly IDistributedCache cache;

        public SubmissionController(
            UserManager<ApplicationUser> userManager,
            ISubmissionService submissionService,
            IProblemService problemService,
            IUtilityService utilityService,
            IContestService contestService,
            ICodeCompareer codeCompareer,
            IDistributedCache cache)
        {
            this.userManager = userManager;
            this.submissionService = submissionService;
            this.problemService = problemService;
            this.utilityService = utilityService;
            this.contestService = contestService;
            this.codeCompareer = codeCompareer;
            this.cache = cache;
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

        [EndpointExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> Create(SubmissionInputModel model)
        {
            if (model.ContestId.HasValue && !contestService.IsActive(model.ContestId.Value))
            {
                return BadRequest(ErrorMessages.ContestIsNotActive);
            }
            if (model.ContestId.HasValue)
            {
                string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (contestService.IsRestricted(model.ContestId.Value, ip))
                {
                    return BadRequest(string.Format(ErrorMessages.DeniedAccessToContestByIp, ip));
                }
            }

            ProblemSubmissionDto problemSubmissionDto = await problemService.GetById<ProblemSubmissionDto>(model.ProblemId);
            int timeIntervalBetweenSubmissionInSeconds = problemSubmissionDto.TimeIntervalBetweenSubmissionInSeconds;
            if (timeIntervalBetweenSubmissionInSeconds >= GlobalConstants.DefaultTimeIntervalBetweenSubmissionInSeconds)
            {
                string key = $"{User.Identity.Name}#{nameof(model.ProblemId)}:{model.ProblemId}";
                string lastSubmissionDateTime = cache.GetString(key);
                if (lastSubmissionDateTime == null)
                {
                    AddClintIpInCache(key, timeIntervalBetweenSubmissionInSeconds);
                }
                else
                {
                    double passedSeconds = DateTime.UtcNow.GetDifferenceInSeconds(lastSubmissionDateTime, GlobalConstants.StandardDateFormat);
                    double secondsToWaitUntilNextSubmission = timeIntervalBetweenSubmissionInSeconds - passedSeconds;
                    if (secondsToWaitUntilNextSubmission > 0)
                    {
                        return BadRequest(string.Format(ErrorMessages.SendSubmissionToEarly, Math.Ceiling(secondsToWaitUntilNextSubmission)));
                    }
                }
            }

            string userId = userManager.GetUserId(User);
            SubmissionDto submission = null;
            if (problemSubmissionDto.TestingStrategy == TestingStrategy.RunAutomatedTests)
            {
                model.SubmissionContent = await model.File.ToArrayAsync();
                submission = await submissionService.Create(model, userId);
                await submissionService.RunAutomatedTests(submission.Id, model.ProgrammingLanguage);
            }
            else
            {
                SubmissionCodeDto submissionCode = await utilityService.ExtractSubmissionCode(model.Code, model.File, model.ProgrammingLanguage);
                model.SubmissionContent = submissionCode.Content;
                submission = await submissionService.Create(model, userId);
                await submissionService.ExecuteSubmission(submission.Id, submissionCode.SourceCodes, model.ProgrammingLanguage);
            }

            await submissionService.CalculateActualPoints(submission.Id);

            SubmissionResult submissionResult = submissionService.GetSubmissionResult(submission.Id);
            return Json(submissionResult);
        }

        private void AddClintIpInCache(string key, int timeIntervalBetweenSubmissionInSeconds)
        {
            string submissionDateTime = DateTime.UtcNow.ToString(GlobalConstants.StandardDateFormat);
            var absoluteExpiration = TimeSpan.FromSeconds(Math.Max(timeIntervalBetweenSubmissionInSeconds, GlobalConstants.DefaultTimeIntervalBetweenSubmissionInSeconds));
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(absoluteExpiration);
            cache.SetString(key, submissionDateTime, options);
        }
    }
}
