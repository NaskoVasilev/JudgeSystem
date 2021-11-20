namespace JudgeSystem.Common.Helpers
{
    public static class UrlHelper
    {
        private const string PathDelimiter = "/";

        public static string Combine(string baseUrl, string route)
        {
            if (baseUrl.EndsWith(PathDelimiter))
            {
                return baseUrl + route;
            }

            return baseUrl + PathDelimiter + route;
        }
    }
}
