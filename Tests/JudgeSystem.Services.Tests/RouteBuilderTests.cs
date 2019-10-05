using JudgeSystem.Common;
using JudgeSystem.Data.Models.Enums;
using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class RouteBuilderTests
    {
        [Theory]
        [InlineData(12, SchoolClassType.A, "classNumber=12&classType=A&page={0}")]
        [InlineData(null, SchoolClassType.A, "classType=A&page={0}")]
        [InlineData(11, null, "classNumber=11&page={0}")]
        [InlineData(null, null, "page={0}")]
        public void BuildStudentByClassRoute_WithDifferentDifferentParamters_ShouldBuildCorrectRoutes(int? classNumber, SchoolClassType? classType, string expectedRoute)
        {
            string methodName = "ByClass";
            expectedRoute = $"/{GlobalConstants.AdministrationArea}/Student/{methodName}?" + expectedRoute;

            string actualRoute = new RouteBuilder().BuildStudentByClassRoute(classNumber, classType, methodName);

            Assert.Equal(expectedRoute, actualRoute);
        }
    }
}
