using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

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

        public static IEnumerable<string> GetEnumDisplayNames<T>()
        {
            foreach (T element in Enum.GetValues(typeof(T)))
            {
                DisplayAttribute displayAttribute = element.GetType()
                        .GetMember(element.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>();
                yield return displayAttribute?.Name ?? element.ToString();
            }
        }
	}
}
