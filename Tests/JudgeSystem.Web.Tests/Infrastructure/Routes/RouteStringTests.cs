using JudgeSystem.Common;
using JudgeSystem.Web.Infrastructure.Routes;

using Xunit;

namespace JudgeSystem.Web.Tests.Infrastructure.Routes
{
    public class RouteStringTests
    {
        private const string ControllerWithSuffix = "ProblemController";
        private const string Controller = "Problem";
        private const string Action = "Create";

        [Fact]
        public void CallConstructor_WithTwoParameters_ShouldGenerateRouteWithControllerAndAction()
        {
            string expectedValue = $"/{Controller}/{Action}";

            var route = new RouteString(ControllerWithSuffix, Action);

            Assert.Equal(expectedValue, route);
            Assert.Equal(expectedValue, route.Value);
        }

        [Fact]
        public void CallConstructor_WithThreeParameters_ShouldGenerateRouteWithAreaControllerAndAction()
        {
            string expectedValue = $"/{GlobalConstants.AdministrationArea}/{Controller}/{Action}";

            var route = new RouteString(GlobalConstants.AdministrationArea, ControllerWithSuffix, Action);

            Assert.Equal(expectedValue, route);
        }

        [Fact]
        public void AppendId_WithPassedIdAsArgument_ShouldAddIdToRoute()
        {
            int id = 4;
            var route = new RouteString(GlobalConstants.AdministrationArea, ControllerWithSuffix, Action);

            route.AppendId(id);

            Assert.Equal($"/{GlobalConstants.AdministrationArea}/{Controller}/{Action}/{id}", route);
        }

        [Fact]
        public void Append_WithNoQueryStringElemenets_SouldAddQueryStringSignAndNewKeyValuePair()
        {
            string key = "name";
            string value = "atanas";
            var route = new RouteString(ControllerWithSuffix, Action);

            route.Append(key, value);

            Assert.Equal($"/{Controller}/{Action}?{key}={value}", route);
            Assert.Equal(1, route.QueryStringPairsCount);
        }


        [Fact]
        public void Append_WithMultipleInvokes_SouldConstrucCorrectQueryString()
        {
            string[] keys = { "name", "memory", "age", "time" };
            object[] values = { "sum", 456.654, 18, 132.6545489 };
            string expectedRoute = $"/{Controller}/{Action}?";
            var route = new RouteString(ControllerWithSuffix, Action);

            for (int i = 0; i < keys.Length; i++)
            {
                route.Append(keys[i], values[i]);
                expectedRoute += keys[i] + "=" + values[i];

                if(i < keys.Length - 1)
                {
                    expectedRoute += GlobalConstants.QueryStringDelimiter;
                }
            }

            Assert.Equal(expectedRoute, route);
            Assert.Equal(keys.Length, route.QueryStringPairsCount);
        }

        [Fact]
        public void AppendPaginationPlaceholder_WithNoQueryParametrs_ShouldAddQueryStringSignAndPageKey()
        {
            var route = new RouteString(ControllerWithSuffix, Action);

            route.AppendPaginationPlaceholder();

            Assert.Equal($"/{Controller}/{Action}?{GlobalConstants.PageKey}={{0}}", route);
            Assert.Equal(1, route.QueryStringPairsCount);
        }

        [Fact]
        public void AppendPaginationPlaceholder_WithQueryParametrs_ShouldAddOnlyPageKey()
        {
            string key = "name";
            string value = "atanas";
            var route = new RouteString(ControllerWithSuffix, Action);

            route.Append(key, value).AppendPaginationPlaceholder();

            Assert.Equal($"/{Controller}/{Action}?{key}={value}{GlobalConstants.QueryStringDelimiter}{GlobalConstants.PageKey}={{0}}", route);
            Assert.Equal(2, route.QueryStringPairsCount);
        }

        [Fact]
        public void RouteString_WithChainedMethods_ShouldConstructCorrectRoute()
        {
            int id = 1;
            string key = "name";
            string value = "atanas";
            var route = new RouteString(ControllerWithSuffix, Action);

            route.AppendId(id).Append(key, value).AppendPaginationPlaceholder();

            Assert.Equal($"/{Controller}/{Action}/{id}?{key}={value}{GlobalConstants.QueryStringDelimiter}{GlobalConstants.PageKey}={{0}}", route);
            Assert.Equal(2, route.QueryStringPairsCount);
        }
    }
}
