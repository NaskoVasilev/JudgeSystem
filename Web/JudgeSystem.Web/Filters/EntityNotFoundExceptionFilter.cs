using JudgeSystem.Common.Exceptions;
using JudgeSystem.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JudgeSystem.Web.Filters
{
    public class EntityNotFoundExceptionFilter : IExceptionFilter
    {
        private const string EntityNotFoundErrorViewName = "EntityNotFoundError";
        private readonly IModelMetadataProvider modelMetadataProvider;

        public EntityNotFoundExceptionFilter(IModelMetadataProvider modelMetadataProvider)
        {
            this.modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            if(context.Exception is EntityNotFoundException)
            {
                var exception = context.Exception as EntityNotFoundException;

                var result = new ViewResult { ViewName = EntityNotFoundErrorViewName };
                result.ViewData = new ViewDataDictionary(modelMetadataProvider, context.ModelState)
                {
                    Model = new EntityNotFoundErrorViewModel { Message = exception.Message}
                };

                context.Result = result;
            }
        }
    }
}
