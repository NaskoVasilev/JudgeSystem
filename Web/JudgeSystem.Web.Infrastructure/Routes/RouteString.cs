using JudgeSystem.Common;
using JudgeSystem.Web.Infrastructure.Extensions;

namespace JudgeSystem.Web.Infrastructure.Routes
{
    public class RouteString
    {
        public int QueryStringPairsCount { get; set; }

        public string Value { get; private set; }

        public RouteString(string controller, string action)
        {
            Value = $"/{controller.ToControllerName()}/{action}";
        }

        public RouteString(string area, string controller, string action) : this(controller, action)
        {
            Value = $"/{area}{Value}";
        }

        public RouteString AppendId(object id)
        {
            Value += $"/{id}";
            return this;
        }

        public RouteString Append(string key, object value)
        {
            if(QueryStringPairsCount == 0)
            {
                AppendQueryStringSymbol();
            }

            string pair = $"{key}={value}";
            QueryStringPairsCount++;

            if(QueryStringPairsCount > 1)
            {
                pair = GlobalConstants.QueryStringDelimiter + pair;
            }

            Value += pair;
            return this;
        }

        public RouteString AppendPaginationPlaceholder() => Append(GlobalConstants.PageKey, "{0}");

        private void AppendQueryStringSymbol() => Value += "?";

        public static implicit operator string(RouteString route) => route.Value;
    }
}
