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

        public static IEnumerable<SelectListItem> GetResourceTypesSelectList()
		{
			return EnumExtensions.GetEnumValuesAsString<ResourceType>()
				.Select(r => new SelectListItem { Value = r, Text = r.InsertSpaceBeforeUppercaseLetter() });
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
    }
}
