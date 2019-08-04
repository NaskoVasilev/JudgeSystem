using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JudgeSystem.Web.Filters
{
    public class EndpointExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string errorMessage = ErrorMessages.EndpointErrorMessage;
            if(context.Exception is EntityNotFoundException exception)
            {
                errorMessage = exception.Message;
            }

            context.Result = new BadRequestObjectResult(errorMessage);
        }
    }
}
