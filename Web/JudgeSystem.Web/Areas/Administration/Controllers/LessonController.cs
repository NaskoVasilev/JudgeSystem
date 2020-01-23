using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.Lesson;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Models.Enums;

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

        public IActionResult Create(LessonType type) => View(new LessonInputModel { Type = type });

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(LessonInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            LessonDto lesson = await lessonService.Create(model);
            int practiceId = await practiceService.Create(lesson.Id);

            return RedirectToAction("Details", "Lesson", new { id = lesson.Id, PracticeId = practiceId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            LessonEditInputModel lesson = await lessonService.GetById<LessonEditInputModel>(id);
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

            LessonDto lesson = await lessonService.Update(model);

            return RedirectToAction("Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
        }

        public IActionResult AddPassword() => View();

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddPassword(LessonAddPasswordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            LessonDto lesson = await lessonService.GetById<LessonDto>(model.Id);

            try
            {
                await lessonService.SetPassword(model.Id, model.LessonPassword);
                string infoMessage = string.Format(InfoMessages.AddLessonPasswordSuccessfully, lesson.Name);
                return ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
            }
            catch (BadRequestException ex)
            {
                return ShowError(ex.Message, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
            }
        }

        public IActionResult ChangePassword() => View();

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
                LessonDto lesson = await lessonService.UpdatePassword(model.Id, model.OldPassword, model.NewPassword);
                string infoMessage = string.Format(InfoMessages.ChangeLessonPasswordSuccessfully, lesson.Name);
                return ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(nameof(LessonChangePasswordInputModel.OldPassword), ex.Message);
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            LessonEditInputModel lesson = await lessonService.GetById<LessonEditInputModel>(id);
            return View(lesson);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            LessonDto lesson = await lessonService.Delete(id);
            return Redirect($"/Course/Lessons?courseId={lesson.CourseId}&lessonType={lesson.Type}");
        }

        public IActionResult RemovePassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePassword(LessonRemovePasswordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                LessonDto lesson = await lessonService.UpdatePassword(model.Id, model.OldPassword, null);
                string infoMessage = string.Format(InfoMessages.LessonPasswordRemoved, lesson.Name);
                return ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(nameof(LessonRemovePasswordInputModel.OldPassword), ex.Message);
                return View(model);
            }
        }
    }
}
