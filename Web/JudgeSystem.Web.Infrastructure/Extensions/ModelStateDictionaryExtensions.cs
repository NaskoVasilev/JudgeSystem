using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JudgeSystem.Web.Infrastructure.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddErrors(this ModelStateDictionary modelState, string key, IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                modelState.AddModelError(key, error);
            }
        }
    }
}
