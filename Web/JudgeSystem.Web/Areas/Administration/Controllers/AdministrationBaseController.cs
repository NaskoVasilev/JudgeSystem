namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	using JudgeSystem.Common;
	using JudgeSystem.Web.Controllers;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;

	//TODO: Only for admin and teachers
	//[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
	[Area("Administration")]
	public class AdministrationBaseController : BaseController
	{
		
	}
}
