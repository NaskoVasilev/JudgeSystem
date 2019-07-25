namespace JudgeSystem.Web.Filters
{
    using JudgeSystem.Common;
    using JudgeSystem.Web.Infrastructure.Exceptions;
    using JudgeSystem.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class EntityNotFoundExceptionFilter : IExceptionFilter
    {
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

                var result = new ViewResult { ViewName = "EntityNotFoundError" };
                result.ViewData = new ViewDataDictionary(modelMetadataProvider, context.ModelState)
                {
                    Model = new EntityNotFoundErrorViewModel { Message = exception.Message}
                };

                context.Result = result;
            }
        }
    }
}
