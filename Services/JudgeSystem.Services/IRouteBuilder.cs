using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Services
{
    public interface IRouteBuilder
    {
        string BuildStudentByClassRoute(int? classNumber, SchoolClassType? classType, string methodName);
    }
}
