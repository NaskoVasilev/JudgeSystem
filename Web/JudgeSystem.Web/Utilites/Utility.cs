namespace JudgeSystem.Web.Utilites
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Web.Infrastructure.Extensions;

	using Microsoft.AspNetCore.Mvc.Rendering;

	public static class Utility
	{
		public static IEnumerable<SelectListItem> GetSelectListItems<T>() where T : Enum
		{
			return EnumExtensions.GetEnumValuesAsString<T>()
				.Select(t => new SelectListItem
				{
					Value = t,
					Text = t
				})
				.ToList();
		}

		public static IEnumerable<SelectListItem> GetResourceTypesSelectList()
		{
			return EnumExtensions.GetEnumValuesAsString<ResourceType>()
				.Select(r => new SelectListItem { Value = r, Text = r.FormatResourceType() });
		}
	}
}
