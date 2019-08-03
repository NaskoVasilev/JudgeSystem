namespace JudgeSystem.Web.Utilites
{
    using System.Collections.Generic;
    using System.Linq;

    using JudgeSystem.Data.Models.Enums;
    using JudgeSystem.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc.Rendering;

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

		public static double ConvertBytesToMegaBytes(long bytes)
		{
			double megabyteInBytes = 1000000;
			return bytes / megabyteInBytes;
		}

        public static double ConvertBytesToKiloBytes(long bytes)
        {
            double kilobyteInBytes = 1000;
            return bytes / kilobyteInBytes;
        }

        public static int ConvertMegaBytesToBytes(double megabytes)
        {
            return (int)(megabytes * 1000 * 1000);
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
