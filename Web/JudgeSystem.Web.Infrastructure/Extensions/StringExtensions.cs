namespace JudgeSystem.Web.Infrastructure.Extensions
{
	using System;

	public static class StringExtensions
	{
		public static string NormalizeFileName(this string name)
		{
			int indexOfExtensionStart = name.LastIndexOf('.');
			return name.Substring(0, Math.Min(name.Length, indexOfExtensionStart));
		}
	}
}
