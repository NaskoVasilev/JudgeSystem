using System.Net;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services;
using JudgeSystem.Web.InputModels.AllowedIpAddress;
using JudgeSystem.Web.ViewModels.AllowedIpAddress;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class AllowedIpAddressController : AdministrationBaseController
    {
        private readonly IAllowedIpAddressService allowedIpAddressService;

        public AllowedIpAddressController(IAllowedIpAddressService allowedIpAddressService)
        {
            this.allowedIpAddressService = allowedIpAddressService;
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AllowedIpAddressInputModel model)
        {
            if(!IPAddress.TryParse(model.Value, out _))
            {
                ModelState.AddModelError(nameof(AllowedIpAddressInputModel.Value), ErrorMessages.InvalidIpAddress);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await allowedIpAddressService.Create(model);
            return RedirectToAction(nameof(All));
        }

        public IActionResult All() => View(allowedIpAddressService.All());

        public async Task<IActionResult> Delete(int id)
        {
            AllowedIpAddressViewModel ipAddress = allowedIpAddressService.GetById<AllowedIpAddressViewModel>(id);
            await allowedIpAddressService.Delete(id);
            return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, $"ip address: {ipAddress.Value}"));
        }
    }
}
