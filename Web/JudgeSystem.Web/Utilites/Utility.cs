using System;
using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Workers.Common;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JudgeSystem.Web.Utilites
{
    public static class Utility
    {
        public static IEnumerable<SelectListItem> GetSelectListItems<T>()
        {
            var items = EnumExtensions.GetEnumValuesAsString<T>()
                .Select(t => new SelectListItem
                {
                    Value = t,
                    Text = t
                })
                .ToList();
            return items;
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
            foreach (object programmingLanguageObject in Enum.GetValues(typeof(ProgrammingLanguage)))
            {
                var programmingLanguage = (ProgrammingLanguage)programmingLanguageObject;
                var item = new SelectListItem() { Value = programmingLanguage.ToString() };
                switch (programmingLanguage)
                {
                    case ProgrammingLanguage.CSharp:
                        item.Text = "C# code";
                        break;
                    case ProgrammingLanguage.Java:
                        item.Text = "Java code";
                        break;
                    case ProgrammingLanguage.CPlusPlus:
                        item.Text = "C++ code";
                        break;
                }

                yield return item;
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
    }
}
