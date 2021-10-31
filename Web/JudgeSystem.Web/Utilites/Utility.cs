using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Workers.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JudgeSystem.Web.Utilites
{
    public static class Utility
    {
        public static IEnumerable<SelectListItem> GetSelectListItems<T>(bool useDisplayNameAttributes = false) where T : Enum
        {
            var values = EnumExtensions.GetEnumValuesAsString<T>().ToList();
            var texts = EnumExtensions.GetEnumDisplayNames<T>().ToList();
            for (int i = 0; i < values.Count; i++)
            {
                yield return new SelectListItem
                {
                    Text = useDisplayNameAttributes ? texts[i] : values[i],
                    Value = values[i]
                };
            }
        }

        public static IEnumerable<SelectListItem> GetFormatedSelectListItems<T>()
        {
            var items = EnumExtensions.GetEnumValuesAsString<T>()
                .Select(t => new SelectListItem
                {
                    Value = t,
                    Text = t.InsertSpaceBeforeUppercaseLetter()
                })
                .ToList();
            return items;
        }

        public static IEnumerable<SelectListItem> GetSelectListOfProgrammingLangugages()
        {
            var programmingLangiagesItems = new List<SelectListItem>();
            foreach (object programmingLanguageObject in Enum.GetValues(typeof(ProgrammingLanguage)))
            {
                var programmingLanguage = (ProgrammingLanguage)programmingLanguageObject;
                var item = new SelectListItem() { Value = programmingLanguage.ToString() };
                switch (programmingLanguage)
                {
                    // Disable c# and java as programming languages for now, because we are using only c++ currently
                    //case ProgrammingLanguage.CSharp:
                    //    item.Text = "C# code";
                    //    break;
                    //case ProgrammingLanguage.Java:
                    //    item.Text = "Java code";
                    //    break;
                    case ProgrammingLanguage.CPlusPlus:
                        item.Text = "C++ code";
                        break;
                }

                if(!string.IsNullOrEmpty(item.Text))
                {
                    programmingLangiagesItems.Add(item);
                }

                return programmingLangiagesItems;
            }
        }

        public static string GetLessonName(string lessonBaseName, LessonType lessonType)
        {
            if (lessonType != LessonType.Exam)
            {
                return lessonBaseName + " - " + lessonType.ToString();
            }
            return lessonBaseName;
        }

        public static string ExtractModelStateErrors(ModelStateDictionary modelState, string separator)
        {
            IEnumerable<string> errors = modelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
            return string.Join(separator, errors);
        }

        public static string GetBaseUrl(HttpContext httpContext) => $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}";

        public static IEnumerable<string> ValidateObject(object obj)
        {
            var validationContext = new ValidationContext(obj);
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            Validator.TryValidateObject(obj, validationContext, results, true);
            foreach (ValidationResult result in results)
            {
                yield return result.ErrorMessage;
            }
        }
    }
}
