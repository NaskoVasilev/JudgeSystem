using JudgeSystem.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	//TODO: Only for admin and teachers
	//[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
	[Area("Administration")]
	public class AdministrationBaseController : Controller
	{
	}
}
