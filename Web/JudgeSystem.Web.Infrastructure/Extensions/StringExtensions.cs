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

        public static string InsertSpaceBeforeUppercaseLetter(this string text)
        {
            string tempText = text;
            int insertedValues = 0;
            for (int i = 1; i < text.Length; i++)
            {
                if(char.IsUpper(text[i]))
                {
                    tempText = tempText.Insert(i + insertedValues, " ");
                    insertedValues++;
                }
            }

            return tempText;
        }
	}
}
