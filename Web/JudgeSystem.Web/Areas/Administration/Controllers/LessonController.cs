using System;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.Lesson;
using JudgeSystem.Services;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Dtos.Lesson;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class LessonController : AdministrationBaseController
    {
        private readonly ILessonService lessonService;
        private readonly IPracticeService practiceService;

        public LessonController(
            ILessonService lessonService,
            IPracticeService practiceService)
        {
            this.lessonService = lessonService;
            this.practiceService = practiceService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(LessonInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var lesson = await lessonService.Create(model);
            int practiceId = await practiceService.Create(lesson.Id);

            return RedirectToAction("Details", "Lesson", new { id = lesson.Id, PracticeId = practiceId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await lessonService.GetById<LessonEditInputModel>(id);
            return View(lesson);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(LessonEditInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var lesson = await lessonService.Update(model);

            return RedirectToAction("Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
        }

        public IActionResult AddPassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddPassword(LessonAddPasswordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var lesson = await lessonService.GetById<LessonDto>(model.Id);

            try
            {
                await lessonService.SetPassword(model.Id, model.LessonPassword);
                string infoMessage = string.Format(InfoMessages.AddPasswordSuccessfully, lesson.Name);
                return this.ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
            }
            catch (ArgumentException ex)
            {
                return ShowError(ex.Message, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(LessonChangePasswordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var lesson = await lessonService.UpdatePassword(model.Id, model.OldPassword, model.NewPassword);
                string infoMessage = string.Format(InfoMessages.ChangePasswordSuccessfully, lesson.Name);
                return this.ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
            }
            catch (ArgumentException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [EndpointExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var lessonName = await lessonService.Delete(id);
            return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, lessonName));
        }

        public IActionResult RemovePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemovePassword(LessonRemovePasswordInputModel model)
        {
            try
            {
                LessonDto lesson = await lessonService.UpdatePassword(model.Id, model.OldPassword, null);
                string infoMessage = string.Format(InfoMessages.PasswordRemoved, lesson.Name);
                return this.ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
            }
            catch (ArgumentException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}
