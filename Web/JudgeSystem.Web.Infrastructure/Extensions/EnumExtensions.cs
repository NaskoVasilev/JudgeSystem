using System;
using System.Collections.Generic;

namespace JudgeSystem.Web.Infrastructure.Extensions
{
	public static class EnumExtensions
	{
		public static IEnumerable<string> GetEnumValuesAsString<T>()
		{
			foreach (T element in Enum.GetValues(typeof(T)))
			{
				yield return element.ToString();
			}
		}
	}
}
