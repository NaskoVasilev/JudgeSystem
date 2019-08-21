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
