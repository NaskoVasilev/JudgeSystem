using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Infrastructure.Extensions;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace JudgeSystem.Web.Utilites
{
    public static class Utility
	{
		public static IEnumerable<SelectListItem> GetSelectListItems<T>()
		{
			return EnumExtensions.GetEnumValuesAsString<T>()
				.Select(t => new SelectListItem
				{
					Value = t,
					Text = t
				})
				.ToList();
		}

        public static IEnumerable<SelectListItem> GetFormatedSelectListItems<T>()
        {
            return EnumExtensions.GetEnumValuesAsString<T>()
                .Select(t => new SelectListItem
                {
                    Value = t,
                    Text = t.InsertSpaceBeforeUppercaseLetter()
                })
                .ToList();
        }

        public static string GetLessonName(string lessonBaseName, LessonType lessonType)
        {
            if(lessonType != LessonType.Exam)
            {
                return lessonBaseName + " - " + lessonType.ToString();
            }
            return lessonBaseName;
        }
    }
}
