using JudgeSystem.Services.Data;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Web.Filters;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    public class ProblemController : BaseController
	{
        private readonly IProblemService problemService;

        public ProblemController(IProblemService problemService)
		{
            this.problemService = problemService;
        }

        [EndpointExceptionFilter]
        public IActionResult Get(int id)
        {
            var problem = problemService.GetById<ProblemConstraintsDto>(id);
            return this.Ok(problem);
        }
	}
}
