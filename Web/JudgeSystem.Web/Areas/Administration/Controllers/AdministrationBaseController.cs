using JudgeSystem.Common;
using JudgeSystem.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
	[Area("Administration")]
	public class AdministrationBaseController : BaseController
	{
	}
}
