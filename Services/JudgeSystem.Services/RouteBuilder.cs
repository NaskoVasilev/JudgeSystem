using JudgeSystem.Common;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Services
{
    public class RouteBuilder : IRouteBuilder
    {
        public string BuildStudentByClassRoute(int? classNumber, SchoolClassType? classType, string methodName)
        {
            string url = $"/{GlobalConstants.AdministrationArea}/Student/{methodName}?";
            if (classNumber.HasValue)
            {
                url += $"{nameof(classNumber)}={classNumber}&";
            }

            if (classType.HasValue)
            {
                url += $"{nameof(classType)}={classType}&";
            }

            url += $"{GlobalConstants.PageKey}={{0}}";
            return url;
        }
    }
}
