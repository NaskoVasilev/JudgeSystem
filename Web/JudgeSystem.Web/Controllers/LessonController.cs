using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Lesson;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Web.InputModels.Lesson;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Dtos.Lesson;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
    public class LessonController : BaseController
    {
        private readonly ILessonService lessonService;
        private readonly IContestService contestService;
        private readonly IPasswordHashService passwordHashService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPracticeService practiceService;

        public LessonController(ILessonService lessonService,
            IContestService contestService,
            IPasswordHashService passwordHashService,
            UserManager<ApplicationUser> userManager,
            IPracticeService practiceService)
        {
            this.lessonService = lessonService;
            this.contestService = contestService;
            this.passwordHashService = passwordHashService;
            this.userManager = userManager;
            this.practiceService = practiceService;
        }

        public async Task<IActionResult> Details(int id, int? contestId, int? practiceId)
        {
            if(contestId.HasValue && !User.IsInRole(GlobalConstants.StudentRoleName) && !User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                TempData[GlobalConstants.InfoKey] = ErrorMessages.ContestsAccessibleOnlyForStudents;
                return Redirect("/");
            }

            LessonViewModel lesson = await lessonService.GetLessonInfo(id);
            lesson.ContestId = contestId;
            await AddUserToContestOrPracticeIfNotAdded(contestId, practiceId);

            if (lesson.IsLocked)
            {
                return GetDetailsViewOrRedirectToEnterPasswordPage(lesson);
            }

            return View(lesson);
        }

        public IActionResult EnterPassword() => View();

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EnterPassword(LessonPasswordInputModel model)
        {
            LessonDto lesson = await lessonService.GetById<LessonDto>(model.Id);

            if (lesson.LessonPassword == passwordHashService.HashPassword(model.LessonPassword))
            {
                HttpContext.Session.SetString(lesson.Id.ToString(), User.Identity.Name);
                if(model.ContestId.HasValue)
                {
                    return RedirectToAction(nameof(Details), new { id = lesson.Id, contestId = model.ContestId.Value });
                }
                return RedirectToAction(nameof(Details), new { id = lesson.Id, practiceId = model.PracticeId.Value });
            }

            ModelState.AddModelError(nameof(LessonPasswordInputModel.LessonPassword), ErrorMessages.InvalidPassword);
            return View(model);
        }

        private async Task AddUserToContestOrPracticeIfNotAdded(int? contestId, int? practiceId)
        {
            string userId = userManager.GetUserId(User);
            if (contestId.HasValue)
            {
                await contestService.AddUserToContestIfNotAdded(userId, contestId.Value);
            }
            else
            {
                if (!practiceId.HasValue)
                {
                    throw new BadRequestException(ErrorMessages.InvalidPracticeId);
                }
                await practiceService.AddUserToPracticeIfNotAdded(userId, practiceId.Value);
            }
        }

        private IActionResult GetDetailsViewOrRedirectToEnterPasswordPage(LessonViewModel lesson)
        {
            string sessionValue = HttpContext.Session.GetString(lesson.Id.ToString());

            if (sessionValue != null && sessionValue == User.Identity.Name)
            {
                return View(lesson);
            }
            else
            {
                if(lesson.ContestId.HasValue)
                {
                    return RedirectToAction(nameof(EnterPassword), new { id = lesson.Id, contestId = lesson.ContestId.Value });
                }
                return RedirectToAction(nameof(EnterPassword), new { id = lesson.Id, practiceId = lesson.PracticeId });
            }
        }
    }
}
